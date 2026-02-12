using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfCQRSDemo.Helpers;
using WpfCQRSDemo.Infrastructure.Services;
using WpfCQRSDemo.ViewModels.Base;
using WpfCQRSDemoApplication.Shared.Contracts.Commands.Lobby;
using WpfCQRSDemoApplication.Shared.Contracts.Queries.Lobby;
using WpfCQRSDemoApplication.Shared.DTOs.Game;
using WpfCQRSDemoApplication.Shared.DTOs.Lobby;

namespace WpfCQRSDemo.ViewModels
{
    public class LobbyViewModel : ViewModelBase
    {
        private string _currentPlayerId;
        private string _currentLobbyId;

        public LobbyViewModel(IInfrastructureServices infrastructure) : base(infrastructure)
        {
            AvailableLobbies = new ObservableCollection<LobbyInfoDto>();
            Players = new ObservableCollection<PlayerDto>();
            
            CreateLobbyCommand = new AsyncRelayCommand(CreateLobbyAsync, () => !string.IsNullOrWhiteSpace(PlayerName) && !IsInLobby);
            JoinLobbyCommand = new AsyncRelayCommand<LobbyInfoDto>(JoinLobbyAsync, (lobby) => !string.IsNullOrWhiteSpace(PlayerName) && !IsInLobby && lobby != null);
            LeaveLobbyCommand = new AsyncRelayCommand(LeaveLobbyAsync, () => IsInLobby);
            ReadyCommand = new AsyncRelayCommand(ToggleReadyAsync, () => IsInLobby);
            StartGameCommand = new AsyncRelayCommand(StartGameAsync, () => CanStartGame);

            SignalR.LobbyListUpdateReceived += OnLobbyListUpdateReceived;
            SignalR.LobbyUpdateReceived += OnLobbyUpdateReceived;
            SignalR.PlayerJoinedReceived += OnPlayerJoinedReceived;
            SignalR.PlayerLeftReceived += OnPlayerLeftReceived;
            SignalR.GameStartedReceived += OnGameStartedReceived;
        }

        // Properties
        public ObservableCollection<LobbyInfoDto> AvailableLobbies { get; }
        public ObservableCollection<PlayerDto> Players { get; }

        public string PlayerName
        {
            get => field;
            set
            {
                if (SetProperty(ref field, value))
                {
                    RaiseCommandsCanExecuteChanged();
                }
            }
        }

        public string LobbyName
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public bool IsInLobby
        {
            get => field;
            set
            {
                if (SetProperty(ref field, value))
                {
                    RaiseCommandsCanExecuteChanged();
                    OnPropertyChanged(nameof(IsNotInLobby));
                }
            }
        }

        public bool IsNotInLobby => !IsInLobby;

        public bool IsHost
        {
            get => field;
            set
            {
                if (SetProperty(ref field, value))
                {
                    RaiseCommandsCanExecuteChanged();
                }
            }
        }

        public bool CanStartGame => IsInLobby && IsHost && Players.Count == 2 && Players.All(p => p.IsReady);

        public LobbyDto CurrentLobby
        {
            get => field;
            set
            {
                if (SetProperty(ref field, value))
                {
                    OnPropertyChanged(nameof(CurrentLobbyName));
                }
            }
        }

        public string CurrentLobbyName => CurrentLobby?.Name ?? string.Empty;

        // Events for navigation
        public event Action<GameStartedDto> GameStarted;

        // Commands
        public AsyncRelayCommand CreateLobbyCommand { get; }
        public AsyncRelayCommand<LobbyInfoDto> JoinLobbyCommand { get; }
        public AsyncRelayCommand LeaveLobbyCommand { get; }
        public AsyncRelayCommand ReadyCommand { get; }
        public AsyncRelayCommand StartGameCommand { get; }

        public override async Task InitializeAsync()
        {
            await SignalR.StartAsync();
            
            // Get initial lobby list
            var lobbyList = await ExecuteQueryAsync(new GetLobbyListQuery());
            if (lobbyList != null)
            {
                UpdateLobbyList(lobbyList);
            }
        }

        private void OnLobbyListUpdateReceived(LobbyListDto update)
        {
            if (update != null)
            {
                UpdateLobbyList(update);
            }
        }

        private void OnLobbyUpdateReceived(LobbyDto update)
        {
            if (update != null && update.Id == _currentLobbyId)
            {
                UpdateCurrentLobby(update);
            }
        }

        private void OnPlayerJoinedReceived(string lobbyId, PlayerDto player)
        {
            if (lobbyId == _currentLobbyId)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (!Players.Any(p => p.Id == player.Id))
                    {
                        Players.Add(player);
                        RaiseCommandsCanExecuteChanged();
                    }
                });
            }
        }

        private void OnPlayerLeftReceived(string lobbyId, string playerId)
        {
            if (lobbyId == _currentLobbyId)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var player = Players.FirstOrDefault(p => p.Id == playerId);
                    if (player != null)
                    {
                        Players.Remove(player);
                        RaiseCommandsCanExecuteChanged();
                    }
                });
            }
        }

        private void OnGameStartedReceived(GameStartedDto gameStarted)
        {
            if (gameStarted.YourPlayerId == _currentPlayerId)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    GameStarted?.Invoke(gameStarted);
                });
            }
        }

        private void UpdateLobbyList(LobbyListDto lobbyList)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                AvailableLobbies.Clear();
                foreach (var lobby in lobbyList.Lobbies)
                {
                    AvailableLobbies.Add(lobby);
                }
            });
        }

        private void UpdateCurrentLobby(LobbyDto lobby)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CurrentLobby = lobby;
                Players.Clear();
                foreach (var player in lobby.Players)
                {
                    Players.Add(player);
                }

                IsHost = lobby.HostPlayerId == _currentPlayerId;
                RaiseCommandsCanExecuteChanged();
            });
        }

        private async Task CreateLobbyAsync()
        {
            var lobbyNameToUse = string.IsNullOrWhiteSpace(LobbyName) 
                ? $"{PlayerName}'s Game" 
                : LobbyName;

            var result = await ExecuteCommandAsync(new CreateLobbyCommand 
            { 
                PlayerName = PlayerName, 
                LobbyName = lobbyNameToUse 
            });

            if (result != null)
            {
                _currentPlayerId = result.PlayerId;
                _currentLobbyId = result.LobbyId;
                IsInLobby = true;
                IsHost = true;

                // Join the SignalR group for this lobby
                await SignalR.JoinLobby(_currentLobbyId, _currentPlayerId, PlayerName);

                // Get current lobby state
                var lobby = await ExecuteQueryAsync(new GetLobbyStatusQuery { LobbyId = _currentLobbyId });
                if (lobby != null)
                {
                    UpdateCurrentLobby(lobby);
                }
            }
        }

        private async Task JoinLobbyAsync(LobbyInfoDto lobbyInfo)
        {
            if (lobbyInfo == null) return;

            var result = await ExecuteCommandAsync(new JoinLobbyCommand 
            { 
                LobbyId = lobbyInfo.Id,
                PlayerName = PlayerName 
            });

            if (result != null && result.Success)
            {
                _currentPlayerId = result.PlayerId;
                _currentLobbyId = lobbyInfo.Id;
                IsInLobby = true;
                IsHost = false;

                // Join the SignalR group for this lobby
                await SignalR.JoinLobby(_currentLobbyId, _currentPlayerId, PlayerName);

                // Get current lobby state
                var lobby = await ExecuteQueryAsync(new GetLobbyStatusQuery { LobbyId = _currentLobbyId });
                if (lobby != null)
                {
                    UpdateCurrentLobby(lobby);
                }
            }
            else if (result != null && !result.Success)
            {
                // Show error
                await DialogService.ShowErrorAsync("Join Failed", result.ErrorMessage);
            }
        }

        private async Task LeaveLobbyAsync()
        {
            if (string.IsNullOrWhiteSpace(_currentPlayerId) || string.IsNullOrWhiteSpace(_currentLobbyId))
                return;

            await SignalR.LeaveLobby(_currentLobbyId);
            await ExecuteCommandAsync(new LeaveLobbyCommand 
            { 
                LobbyId = _currentLobbyId, 
                PlayerId = _currentPlayerId 
            });
            
            ResetLobbyState();
        }

        private async Task ToggleReadyAsync()
        {
            if (string.IsNullOrWhiteSpace(_currentPlayerId) || string.IsNullOrWhiteSpace(_currentLobbyId))
                return;

            var current = Players.FirstOrDefault(p => p.Id == _currentPlayerId);
            var nextReady = current == null || !current.IsReady;
            
            await ExecuteCommandAsync(new SetPlayerReadyCommand 
            { 
                LobbyId = _currentLobbyId,
                PlayerId = _currentPlayerId, 
                IsReady = nextReady 
            });
        }

        private async Task StartGameAsync()
        {
            if (string.IsNullOrWhiteSpace(_currentPlayerId) || string.IsNullOrWhiteSpace(_currentLobbyId))
                return;

            try
            {
                var gameStarted = await ExecuteCommandAsync(new StartGameCommand 
                { 
                    LobbyId = _currentLobbyId, 
                    PlayerId = _currentPlayerId 
                });

                // The GameStartedReceived event will handle navigation
            }
            catch (Exception ex)
            {
                await DialogService.ShowErrorAsync("Start Game Failed", ex.Message);
            }
        }

        private void ResetLobbyState()
        {
            _currentPlayerId = null;
            _currentLobbyId = null;
            IsInLobby = false;
            IsHost = false;
            CurrentLobby = null;
            Players.Clear();
        }

        private void RaiseCommandsCanExecuteChanged()
        {
            CreateLobbyCommand.RaiseCanExecuteChanged();
            JoinLobbyCommand.RaiseCanExecuteChanged();
            LeaveLobbyCommand.RaiseCanExecuteChanged();
            ReadyCommand.RaiseCanExecuteChanged();
            StartGameCommand.RaiseCanExecuteChanged();
            OnPropertyChanged(nameof(CanStartGame));
        }

        public override async Task OnNavigatedFromAsync()
        {
            SignalR.LobbyListUpdateReceived -= OnLobbyListUpdateReceived;
            SignalR.LobbyUpdateReceived -= OnLobbyUpdateReceived;
            SignalR.PlayerJoinedReceived -= OnPlayerJoinedReceived;
            SignalR.PlayerLeftReceived -= OnPlayerLeftReceived;
            SignalR.GameStartedReceived -= OnGameStartedReceived;

            if (IsInLobby && !string.IsNullOrWhiteSpace(_currentLobbyId))
            {
                await SignalR.LeaveLobby(_currentLobbyId);
            }
        }

        // Public accessor for game navigation
        public string CurrentPlayerId => _currentPlayerId;
    }
}
