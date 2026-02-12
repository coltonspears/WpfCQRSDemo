using Microsoft.AspNetCore.SignalR;
using WpfCQRSDemoApplication.Server.Domain.Lobby;
using WpfCQRSDemoApplication.Server.Domain.TicTacToe;
using WpfCQRSDemoApplication.Shared.Infrastructure.SignalR;

namespace WpfCQRSDemoApplication.Server.Infrastructure.SignalR;

public class LiveUpdateHub : Hub<ILiveUpdateClient>
{
    private readonly IConnectionManager _connectionManager;
    private readonly ILobbyStore _lobbyStore;
    private readonly IGameStore _gameStore;

    public LiveUpdateHub(
        IConnectionManager connectionManager,
        ILobbyStore lobbyStore,
        IGameStore gameStore)
    {
        _connectionManager = connectionManager;
        _lobbyStore = lobbyStore;
        _gameStore = gameStore;
    }

    public override async Task OnConnectedAsync()
    {
        _connectionManager.OnConnected(Context.ConnectionId);
        
        // Automatically join lobby list group to receive lobby updates
        await Groups.AddToGroupAsync(Context.ConnectionId, "lobby_list");
        
        // Send current lobby list to newly connected client
        await Clients.Caller.ReceiveLobbyListUpdate(_lobbyStore.GetLobbyList());
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connection = _connectionManager.GetConnection(Context.ConnectionId);
        
        if (connection != null)
        {
            // If player was in a lobby, leave it
            if (!string.IsNullOrEmpty(connection.CurrentLobbyId) && !string.IsNullOrEmpty(connection.PlayerId))
            {
                var wasInLobby = _lobbyStore.TryLeaveLobby(connection.CurrentLobbyId, connection.PlayerId);
                if (wasInLobby)
                {
                    var lobby = _lobbyStore.GetLobby(connection.CurrentLobbyId);
                    if (lobby != null)
                    {
                        await Clients.Group($"lobby_{connection.CurrentLobbyId}")
                            .ReceiveLobbyUpdate(lobby);
                        await Clients.Group($"lobby_{connection.CurrentLobbyId}")
                            .ReceivePlayerLeft(connection.CurrentLobbyId, connection.PlayerId);
                    }
                    
                    // Update lobby list for all
                    await Clients.Group("lobby_list").ReceiveLobbyListUpdate(_lobbyStore.GetLobbyList());
                }
            }
            
            // If player was in a game, handle forfeit
            if (!string.IsNullOrEmpty(connection.CurrentGameId) && !string.IsNullOrEmpty(connection.PlayerId))
            {
                var game = _gameStore.GetGame(connection.CurrentGameId);
                if (game != null && game.Status == Shared.DTOs.Game.GameStatus.InProgress)
                {
                    // Determine winner (the player who didn't disconnect)
                    string winnerId = null;
                    string winnerName = null;
                    
                    if (game.PlayerX?.Id == connection.PlayerId)
                    {
                        winnerId = game.PlayerO?.Id;
                        winnerName = game.PlayerO?.Name;
                    }
                    else if (game.PlayerO?.Id == connection.PlayerId)
                    {
                        winnerId = game.PlayerX?.Id;
                        winnerName = game.PlayerX?.Name;
                    }
                    
                    await Clients.Group($"game_{connection.CurrentGameId}")
                        .ReceiveGameEnded(connection.CurrentGameId, winnerId, $"{winnerName} (opponent disconnected)");
                    
                    _gameStore.TryRemoveGame(connection.CurrentGameId);
                }
            }
        }
        
        _connectionManager.OnDisconnected(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinLobbyList()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "lobby_list");
        await Clients.Caller.ReceiveLobbyListUpdate(_lobbyStore.GetLobbyList());
    }

    public async Task LeaveLobbyList()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "lobby_list");
    }

    public async Task JoinLobby(string lobbyId, string playerId, string playerName)
    {
        _connectionManager.SetPlayerInfo(Context.ConnectionId, playerId, playerName);
        _connectionManager.SetLobbyId(Context.ConnectionId, lobbyId);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, $"lobby_{lobbyId}");
        
        var lobby = _lobbyStore.GetLobby(lobbyId);
        if (lobby != null)
        {
            await Clients.Caller.ReceiveLobbyUpdate(lobby);
        }
    }

    public async Task LeaveLobby(string lobbyId)
    {
        var connection = _connectionManager.GetConnection(Context.ConnectionId);
        
        if (connection != null && !string.IsNullOrEmpty(connection.PlayerId))
        {
            _lobbyStore.TryLeaveLobby(lobbyId, connection.PlayerId);
            
            var lobby = _lobbyStore.GetLobby(lobbyId);
            if (lobby != null)
            {
                await Clients.Group($"lobby_{lobbyId}").ReceiveLobbyUpdate(lobby);
                await Clients.Group($"lobby_{lobbyId}").ReceivePlayerLeft(lobbyId, connection.PlayerId);
            }
            
            await Clients.Group("lobby_list").ReceiveLobbyListUpdate(_lobbyStore.GetLobbyList());
        }
        
        _connectionManager.ClearLobbyId(Context.ConnectionId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"lobby_{lobbyId}");
    }

    public async Task JoinGame(string gameId)
    {
        _connectionManager.SetGameId(Context.ConnectionId, gameId);
        await Groups.AddToGroupAsync(Context.ConnectionId, $"game_{gameId}");
    }

    public async Task LeaveGame(string gameId)
    {
        _connectionManager.ClearGameId(Context.ConnectionId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"game_{gameId}");
    }

    public async Task SetPlayerInfo(string playerId, string playerName)
    {
        _connectionManager.SetPlayerInfo(Context.ConnectionId, playerId, playerName);
        await Task.CompletedTask;
    }
}
