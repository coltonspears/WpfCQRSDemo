using System.Collections.Generic;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;

namespace WpfCQRSDemoApplication.Shared.DTOs.Leaderboard
{
    public class LeaderboardDto
    {
        public List<LeaderboardEntryDto> Entries { get; set; } = new List<LeaderboardEntryDto>();
    }

    public class LeaderboardEntryDto
    {
        public string PlayerName { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int Points => (Wins * 3) + Draws;
    }
}

namespace WpfCQRSDemoApplication.Shared.Contracts.Queries.Leaderboard
{
    public class GetLeaderboardQuery : IQuery<WpfCQRSDemoApplication.Shared.DTOs.Leaderboard.LeaderboardDto>
    {
        public bool ExecuteOnServer => true;
    }
}
