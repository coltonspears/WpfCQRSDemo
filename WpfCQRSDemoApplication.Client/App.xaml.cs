using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfCQRSDemo.ViewModels;
using WpfCQRSDemo.Views;
using WpfCQRSDemoApplication.Shared.DTOs.Game;

namespace WpfCQRSDemo
{
    public partial class App : Application
    {
        private System.IServiceProvider _serviceProvider;
        private TabControl _tabs;
        private TicTacToeViewModel _ticTacToeViewModel;
        private LobbyViewModel _lobbyViewModel;
        private LeaderboardViewModel _leaderboardViewModel;
        private TabItem _gameTab;
        private TabItem _lobbyTab;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootstrapper = new Bootstrapper();
            _serviceProvider = bootstrapper.ConfigureServices();
            var mainWindow = new MainWindow();

            _ticTacToeViewModel = (TicTacToeViewModel)_serviceProvider.GetService(typeof(TicTacToeViewModel));
            _lobbyViewModel = (LobbyViewModel)_serviceProvider.GetService(typeof(LobbyViewModel));
            _leaderboardViewModel = (LeaderboardViewModel)_serviceProvider.GetService(typeof(LeaderboardViewModel));

            var ticTacToeView = new TicTacToeView { DataContext = _ticTacToeViewModel };
            var lobbyView = new LobbyView { DataContext = _lobbyViewModel };
            var leaderboardView = new LeaderboardView { DataContext = _leaderboardViewModel };

            _tabs = new TabControl();
            _lobbyTab = new TabItem { Header = "Lobby", Content = lobbyView };
            _gameTab = new TabItem { Header = "Game", Content = ticTacToeView };
            var leaderboardTab = new TabItem { Header = "Leaderboard", Content = leaderboardView };

            _tabs.Items.Add(_lobbyTab);
            _tabs.Items.Add(_gameTab);
            _tabs.Items.Add(leaderboardTab);

            // Start on the Lobby tab
            _tabs.SelectedItem = _lobbyTab;

            mainWindow.Content = _tabs;
            mainWindow.Title = "WPF CQRS Demo - Tic Tac Toe";
            mainWindow.Height = 600;
            mainWindow.Width = 900;
            mainWindow.Show();

            // Subscribe to game started event from lobby
            _lobbyViewModel.GameStarted += OnGameStarted;
            
            // Subscribe to game left event from game
            _ticTacToeViewModel.GameLeft += OnGameLeft;

            // Initialize lobby and leaderboard (game will be initialized when started)
            await Task.WhenAll(
                _lobbyViewModel.InitializeAsync(),
                _leaderboardViewModel.InitializeAsync());
        }

        private async void OnGameStarted(GameStartedDto gameStarted)
        {
            await Current.Dispatcher.InvokeAsync(async () =>
            {
                // Initialize game view model with game data
                await _ticTacToeViewModel.InitializeFromGameStartedAsync(gameStarted);
                
                // Switch to game tab
                _tabs.SelectedItem = _gameTab;
            });
        }

        private void OnGameLeft()
        {
            Current.Dispatcher.Invoke(() =>
            {
                // Switch back to lobby tab
                _tabs.SelectedItem = _lobbyTab;
            });
        }
    }
}
