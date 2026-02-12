using WpfCQRSDemoApplication.Shared.Contracts.Queries.Leaderboard;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.DTOs.Leaderboard;

namespace WpfCQRSDemoApplication.Server.Handlers.Leaderboard;

public class LeaderboardHandlers : IQueryHandler<GetLeaderboardQuery, LeaderboardDto>
{
    public Task<LeaderboardDto> HandleAsync(GetLeaderboardQuery query)
    {
        // Mock data for demo
        var leaderboard = new LeaderboardDto
        {
            Entries = new List<LeaderboardEntryDto>
            {
                new LeaderboardEntryDto { PlayerName = "Alice", Wins = 10, Losses = 2, Draws = 1 },
                new LeaderboardEntryDto { PlayerName = "Bob", Wins = 8, Losses = 4, Draws = 2 },
                new LeaderboardEntryDto { PlayerName = "Charlie", Wins = 5, Losses = 5, Draws = 3 }
            }
        };
        return Task.FromResult(leaderboard);
    }
}
