using System.Threading.Tasks;
using WpfCQRSDemoApplication.Shared.DTOs.Game;
using WpfCQRSDemoApplication.Shared.DTOs.Leaderboard;
using WpfCQRSDemoApplication.Shared.DTOs.Lobby;
using WpfCQRSDemoApplication.Shared.DTOs.TicTacToe;

namespace WpfCQRSDemoApplication.Shared.Infrastructure.SignalR
{
    public interface ILiveUpdateClient
    {
        // Legacy game update (kept for compatibility)
        Task ReceiveGameUpdate(string gameId, BoardStatusDto update);

        // Leaderboard updates
        Task ReceiveLeaderboardUpdate(LeaderboardDto update);

        // Lobby updates
        Task ReceiveLobbyUpdate(LobbyDto update);
        Task ReceiveLobbyListUpdate(LobbyListDto update);

        // Game lifecycle events
        Task ReceiveGameStarted(GameStartedDto gameStarted);
        Task ReceiveGameStateUpdate(GameUpdateDto update);
        Task ReceiveGameEnded(string gameId, string winnerId, string winnerName);

        // Player events
        Task ReceivePlayerJoined(string lobbyId, PlayerDto player);
        Task ReceivePlayerLeft(string lobbyId, string playerId);
    }
}
