using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WpfCQRSDemo.Infrastructure.Services;
using WpfCQRSDemo.ViewModels.Base;
using WpfCQRSDemoApplication.Shared.Contracts.Queries.Leaderboard;
using WpfCQRSDemoApplication.Shared.DTOs.Leaderboard;

namespace WpfCQRSDemo.ViewModels
{
    public class LeaderboardViewModel : ViewModelBase
    {
        private ObservableCollection<LeaderboardEntryDto> _entries = new ObservableCollection<LeaderboardEntryDto>();

        public LeaderboardViewModel(IInfrastructureServices infrastructure) : base(infrastructure)
        {
            Entries = new ObservableCollection<LeaderboardEntryDto>();
            SignalR.LeaderboardUpdateReceived += OnLeaderboardUpdateReceived;
        }

        public ObservableCollection<LeaderboardEntryDto> Entries
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public override async Task InitializeAsync()
        {
            await SignalR.StartAsync();
            var leaderboard = await ExecuteQueryAsync(new GetLeaderboardQuery());
            if (leaderboard != null)
            {
                UpdateLeaderboard(leaderboard);
            }
        }

        private void OnLeaderboardUpdateReceived(LeaderboardDto update)
        {
            if (update != null)
            {
                UpdateLeaderboard(update);
            }
        }

        private void UpdateLeaderboard(LeaderboardDto leaderboard)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                Entries.Clear();
                foreach (var entry in leaderboard.Entries)
                {
                    Entries.Add(entry);
                }
            });
        }

        public override Task OnNavigatedFromAsync()
        {
            SignalR.LeaderboardUpdateReceived -= OnLeaderboardUpdateReceived;
            return Task.FromResult(true);
        }
    }
}
