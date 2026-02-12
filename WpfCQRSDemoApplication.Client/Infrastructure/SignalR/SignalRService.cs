using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using WpfCQRSDemoApplication.Shared.DTOs.Game;
using WpfCQRSDemoApplication.Shared.DTOs.Leaderboard;
using WpfCQRSDemoApplication.Shared.DTOs.Lobby;
using WpfCQRSDemoApplication.Shared.DTOs.TicTacToe;
using WpfCQRSDemoApplication.Shared.Infrastructure.SignalR;

namespace WpfCQRSDemoApplication.Client.Infrastructure.SignalR
{
    public interface ISignalRService
    {
        Task StartAsync();
        Task StopAsync();
        HubConnectionState State { get; }
        
        // Legacy events
        event Action<string, BoardStatusDto> GameUpdateReceived;
        event Action<LeaderboardDto> LeaderboardUpdateReceived;
        
        // Lobby events
        event Action<LobbyDto> LobbyUpdateReceived;
        event Action<LobbyListDto> LobbyListUpdateReceived;
        event Action<string, PlayerDto> PlayerJoinedReceived;
        event Action<string, string> PlayerLeftReceived;
        
        // Game events
        event Action<GameStartedDto> GameStartedReceived;
        event Action<GameUpdateDto> GameStateUpdateReceived;
        event Action<string, string, string> GameEndedReceived; // gameId, winnerId, winnerName

        // Lobby methods
        Task JoinLobbyList();
        Task LeaveLobbyList();
        Task JoinLobby(string lobbyId, string playerId, string playerName);
        Task LeaveLobby(string lobbyId);
        
        // Game methods
        Task JoinGame(string gameId);
        Task LeaveGame(string gameId);
        
        // Player info
        Task SetPlayerInfo(string playerId, string playerName);
    }

    public class SignalRService : ISignalRService, IAsyncDisposable
    {
        private readonly HubConnection _connection;
        private readonly string _hubUrl;

        public SignalRService(string baseUrl)
        {
            _hubUrl = $"{baseUrl.TrimEnd('/')}/hubs/live";
            _connection = new HubConnectionBuilder()
                .WithUrl(_hubUrl)
                .WithAutomaticReconnect()
                .Build();

            // Legacy events
            _connection.On<string, BoardStatusDto>("ReceiveGameUpdate", (gameId, update) =>
                GameUpdateReceived?.Invoke(gameId, update));
            
            _connection.On<LeaderboardDto>("ReceiveLeaderboardUpdate", (update) =>
                LeaderboardUpdateReceived?.Invoke(update));
            
            // Lobby events
            _connection.On<LobbyDto>("ReceiveLobbyUpdate", (update) =>
                LobbyUpdateReceived?.Invoke(update));
            
            _connection.On<LobbyListDto>("ReceiveLobbyListUpdate", (update) =>
                LobbyListUpdateReceived?.Invoke(update));
            
            _connection.On<string, PlayerDto>("ReceivePlayerJoined", (lobbyId, player) =>
                PlayerJoinedReceived?.Invoke(lobbyId, player));
            
            _connection.On<string, string>("ReceivePlayerLeft", (lobbyId, playerId) =>
                PlayerLeftReceived?.Invoke(lobbyId, playerId));
            
            // Game events
            _connection.On<GameStartedDto>("ReceiveGameStarted", (gameStarted) =>
                GameStartedReceived?.Invoke(gameStarted));
            
            _connection.On<GameUpdateDto>("ReceiveGameStateUpdate", (update) =>
                GameStateUpdateReceived?.Invoke(update));
            
            _connection.On<string, string, string>("ReceiveGameEnded", (gameId, winnerId, winnerName) =>
                GameEndedReceived?.Invoke(gameId, winnerId, winnerName));
        }

        public HubConnectionState State => _connection.State;

        // Legacy events
        public event Action<string, BoardStatusDto> GameUpdateReceived;
        public event Action<LeaderboardDto> LeaderboardUpdateReceived;
        
        // Lobby events
        public event Action<LobbyDto> LobbyUpdateReceived;
        public event Action<LobbyListDto> LobbyListUpdateReceived;
        public event Action<string, PlayerDto> PlayerJoinedReceived;
        public event Action<string, string> PlayerLeftReceived;
        
        // Game events
        public event Action<GameStartedDto> GameStartedReceived;
        public event Action<GameUpdateDto> GameStateUpdateReceived;
        public event Action<string, string, string> GameEndedReceived;

        public async Task StartAsync()
        {
            if (_connection.State == HubConnectionState.Disconnected)
            {
                await _connection.StartAsync();
            }
        }

        public async Task StopAsync()
        {
            if (_connection.State != HubConnectionState.Disconnected)
            {
                await _connection.StopAsync();
            }
        }

        // Lobby methods
        public async Task JoinLobbyList() => await _connection.InvokeAsync("JoinLobbyList");
        public async Task LeaveLobbyList() => await _connection.InvokeAsync("LeaveLobbyList");
        public async Task JoinLobby(string lobbyId, string playerId, string playerName) => 
            await _connection.InvokeAsync("JoinLobby", lobbyId, playerId, playerName);
        public async Task LeaveLobby(string lobbyId) => await _connection.InvokeAsync("LeaveLobby", lobbyId);
        
        // Game methods
        public async Task JoinGame(string gameId) => await _connection.InvokeAsync("JoinGame", gameId);
        public async Task LeaveGame(string gameId) => await _connection.InvokeAsync("LeaveGame", gameId);
        
        // Player info
        public async Task SetPlayerInfo(string playerId, string playerName) => 
            await _connection.InvokeAsync("SetPlayerInfo", playerId, playerName);

        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
            {
                await _connection.DisposeAsync();
            }
        }
    }
}
