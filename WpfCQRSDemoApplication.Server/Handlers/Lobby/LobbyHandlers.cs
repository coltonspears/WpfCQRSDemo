using Microsoft.AspNetCore.SignalR;
using WpfCQRSDemoApplication.Server.Domain.Lobby;
using WpfCQRSDemoApplication.Server.Domain.TicTacToe;
using WpfCQRSDemoApplication.Server.Infrastructure.SignalR;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Commands.Lobby;
using WpfCQRSDemoApplication.Shared.Contracts.Queries.Lobby;
using WpfCQRSDemoApplication.Shared.DTOs.Game;
using WpfCQRSDemoApplication.Shared.DTOs.Lobby;
using WpfCQRSDemoApplication.Shared.Infrastructure.SignalR;

namespace WpfCQRSDemoApplication.Server.Handlers.Lobby;

public class LobbyHandlers : 
    ICommandHandler<CreateLobbyCommand, CreateLobbyResultDto>,
    ICommandHandler<JoinLobbyCommand, JoinLobbyResultDto>,
    ICommandHandler<LeaveLobbyCommand>,
    ICommandHandler<SetPlayerReadyCommand>,
    ICommandHandler<StartGameCommand, GameStartedDto>,
    IQueryHandler<GetLobbyStatusQuery, LobbyDto>,
    IQueryHandler<GetLobbyListQuery, LobbyListDto>
{
    private readonly ILobbyStore _lobbyStore;
    private readonly IGameStore _gameStore;
    private readonly IHubContext<LiveUpdateHub, ILiveUpdateClient> _hubContext;

    public LobbyHandlers(
        ILobbyStore lobbyStore, 
        IGameStore gameStore,
        IHubContext<LiveUpdateHub, ILiveUpdateClient> hubContext)
    {
        _lobbyStore = lobbyStore;
        _gameStore = gameStore;
        _hubContext = hubContext;
    }

    public async Task<CreateLobbyResultDto> HandleAsync(CreateLobbyCommand command)
    {
        var playerId = Guid.NewGuid().ToString();
        var player = new PlayerDto 
        { 
            Id = playerId, 
            Name = command.PlayerName, 
            IsReady = false,
            AssignedMark = null
        };
        
        var lobby = _lobbyStore.CreateLobby(command.LobbyName, player);
        
        // Notify all clients about the new lobby
        await _hubContext.Clients.Group("lobby_list").ReceiveLobbyListUpdate(_lobbyStore.GetLobbyList());
        
        return new CreateLobbyResultDto 
        { 
            LobbyId = lobby.Id, 
            PlayerId = playerId 
        };
    }

    public async Task<JoinLobbyResultDto> HandleAsync(JoinLobbyCommand command)
    {
        var playerId = Guid.NewGuid().ToString();
        var player = new PlayerDto 
        { 
            Id = playerId, 
            Name = command.PlayerName, 
            IsReady = false,
            AssignedMark = null
        };
        
        if (!_lobbyStore.TryJoinLobby(command.LobbyId, player, out var errorMessage))
        {
            return new JoinLobbyResultDto 
            { 
                Success = false, 
                ErrorMessage = errorMessage 
            };
        }
        
        var lobby = _lobbyStore.GetLobby(command.LobbyId);
        
        // Notify players in the lobby
        await _hubContext.Clients.Group($"lobby_{command.LobbyId}").ReceiveLobbyUpdate(lobby);
        await _hubContext.Clients.Group($"lobby_{command.LobbyId}").ReceivePlayerJoined(command.LobbyId, player);
        
        // Notify all clients about updated lobby list
        await _hubContext.Clients.Group("lobby_list").ReceiveLobbyListUpdate(_lobbyStore.GetLobbyList());
        
        return new JoinLobbyResultDto 
        { 
            PlayerId = playerId, 
            Success = true 
        };
    }

    public async Task HandleAsync(LeaveLobbyCommand command)
    {
        var lobby = _lobbyStore.GetLobby(command.LobbyId);
        var wasRemoved = _lobbyStore.TryLeaveLobby(command.LobbyId, command.PlayerId);
        
        if (wasRemoved)
        {
            // Notify remaining players
            var updatedLobby = _lobbyStore.GetLobby(command.LobbyId);
            if (updatedLobby != null)
            {
                await _hubContext.Clients.Group($"lobby_{command.LobbyId}").ReceiveLobbyUpdate(updatedLobby);
                await _hubContext.Clients.Group($"lobby_{command.LobbyId}").ReceivePlayerLeft(command.LobbyId, command.PlayerId);
            }
            
            // Notify all clients about updated lobby list
            await _hubContext.Clients.Group("lobby_list").ReceiveLobbyListUpdate(_lobbyStore.GetLobbyList());
        }
    }

    public async Task HandleAsync(SetPlayerReadyCommand command)
    {
        _lobbyStore.SetPlayerReady(command.LobbyId, command.PlayerId, command.IsReady);
        
        var lobby = _lobbyStore.GetLobby(command.LobbyId);
        if (lobby != null)
        {
            await _hubContext.Clients.Group($"lobby_{command.LobbyId}").ReceiveLobbyUpdate(lobby);
        }
    }

    public async Task<GameStartedDto> HandleAsync(StartGameCommand command)
    {
        var lobby = _lobbyStore.GetLobby(command.LobbyId);
        
        if (lobby == null)
        {
            throw new InvalidOperationException("Lobby not found.");
        }
        
        if (lobby.HostPlayerId != command.PlayerId)
        {
            throw new InvalidOperationException("Only the host can start the game.");
        }
        
        if (lobby.Players.Count != 2)
        {
            throw new InvalidOperationException("Need exactly 2 players to start the game.");
        }
        
        if (!lobby.Players.All(p => p.IsReady))
        {
            throw new InvalidOperationException("All players must be ready.");
        }
        
        // Assign marks - host is X (goes first), other player is O
        var playerX = lobby.Players.First(p => p.Id == lobby.HostPlayerId);
        var playerO = lobby.Players.First(p => p.Id != lobby.HostPlayerId);
        
        _lobbyStore.AssignPlayerMark(command.LobbyId, playerX.Id, "X");
        _lobbyStore.AssignPlayerMark(command.LobbyId, playerO.Id, "O");
        
        // Create the game
        var game = _gameStore.CreateGame(command.LobbyId, playerX, playerO);
        
        // Update lobby status
        _lobbyStore.SetLobbyGameId(command.LobbyId, game.Id);
        _lobbyStore.SetLobbyStatus(command.LobbyId, LobbyStatus.InGame);
        
        // Notify Player X (host)
        var gameStartedX = new GameStartedDto
        {
            GameId = game.Id,
            YourPlayerId = playerX.Id,
            YourMark = "X",
            IsYourTurn = true,
            OpponentName = playerO.Name,
            Board = game.Board
        };
        
        // Notify Player O
        var gameStartedO = new GameStartedDto
        {
            GameId = game.Id,
            YourPlayerId = playerO.Id,
            YourMark = "O",
            IsYourTurn = false,
            OpponentName = playerX.Name,
            Board = game.Board
        };
        
        // Send to both players (they'll filter by their own ID)
        await _hubContext.Clients.Group($"lobby_{command.LobbyId}").ReceiveGameStarted(gameStartedX);
        await _hubContext.Clients.Group($"lobby_{command.LobbyId}").ReceiveGameStarted(gameStartedO);
        
        // Update lobby list (lobby is now in game)
        await _hubContext.Clients.Group("lobby_list").ReceiveLobbyListUpdate(_lobbyStore.GetLobbyList());
        
        // Return the host's game started info
        return gameStartedX;
    }

    public Task<LobbyDto> HandleAsync(GetLobbyStatusQuery query)
    {
        return Task.FromResult(_lobbyStore.GetLobby(query.LobbyId));
    }

    public Task<LobbyListDto> HandleAsync(GetLobbyListQuery query)
    {
        return Task.FromResult(_lobbyStore.GetLobbyList());
    }
}
