using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfCQRSDemo.Helpers;
using WpfCQRSDemo.ViewModels.Base;
using WpfCQRSDemoApplication.Shared.Contracts.Commands.Game;
using WpfCQRSDemoApplication.Shared.DTOs.Game;
using WpfCQRSDemo.Infrastructure.Services;

namespace WpfCQRSDemo.ViewModels
{
    public class TicTacToeViewModel : ViewModelBase
    {
        private string _gameId;
        private string _myPlayerId;
        private string _myMark;
        private bool _isMyTurn;

        public TicTacToeViewModel(IInfrastructureServices infrastructure)
            : base(infrastructure)
        {
            Winner = ".";
            StatusMessage = "Waiting for game...";
            Board = new ObservableCollection<ObservableCollection<string>>();
            InitializeEmptyBoard();

            CellClickCommand = new AsyncRelayCommand<string>(CellClickFromParameterAsync, CanClickCell);
            ResetCommand = new AsyncRelayCommand(ResetAsync, () => !IsBusy && Winner != ".");
            LeaveGameCommand = new AsyncRelayCommand(LeaveGameAsync, () => !IsBusy);

            SignalR.GameStateUpdateReceived += OnGameStateUpdateReceived;
            SignalR.GameEndedReceived += OnGameEndedReceived;
        }

        // Properties
        public ObservableCollection<ObservableCollection<string>> Board
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string Winner
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string StatusMessage
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string MyMark
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string OpponentName
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public bool IsMyTurn
        {
            get => _isMyTurn;
            set
            {
                if (SetProperty(ref _isMyTurn, value))
                {
                    UpdateStatusMessage();
                    CellClickCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsGameOver => Winner != ".";

        // Events
        public event Action GameLeft;

        // Commands
        public AsyncRelayCommand<string> CellClickCommand { get; }
        public AsyncRelayCommand ResetCommand { get; }
        public AsyncRelayCommand LeaveGameCommand { get; }

        // Initialize from GameStartedDto (called from navigation)
        public async Task InitializeFromGameStartedAsync(GameStartedDto gameStarted)
        {
            _gameId = gameStarted.GameId;
            _myPlayerId = gameStarted.YourPlayerId;
            _myMark = gameStarted.YourMark;
            _isMyTurn = gameStarted.IsYourTurn;

            MyMark = _myMark;
            OpponentName = gameStarted.OpponentName;

            await SignalR.StartAsync();
            await SignalR.JoinGame(_gameId);

            ApplyBoard(gameStarted.Board);
            UpdateStatusMessage();
            RaiseAllCanExecuteChanged();
        }

        public override async Task InitializeAsync()
        {
            await SignalR.StartAsync();
            // Note: In the new flow, InitializeFromGameStartedAsync is called instead
        }

        private void OnGameStateUpdateReceived(GameUpdateDto update)
        {
            if (update == null || update.GameId != _gameId)
                return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                ApplyBoard(update.Board);
                Winner = update.Winner ?? ".";
                IsMyTurn = update.CurrentTurnPlayerId == _myPlayerId;
                
                UpdateStatusMessage();
                RaiseAllCanExecuteChanged();
            });
        }

        private void OnGameEndedReceived(string gameId, string winnerId, string winnerName)
        {
            if (gameId != _gameId)
                return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (winnerId == _myPlayerId)
                {
                    StatusMessage = "You win!";
                }
                else if (winnerName == "Draw")
                {
                    StatusMessage = "It's a draw!";
                }
                else
                {
                    StatusMessage = $"{winnerName ?? "Opponent"} wins!";
                }

                Winner = winnerName?.Contains("disconnected") == true ? "Forfeit" : (winnerId == _myPlayerId ? _myMark : (_myMark == "X" ? "O" : "X"));
                IsMyTurn = false;
                RaiseAllCanExecuteChanged();
            });
        }

        private void InitializeEmptyBoard()
        {
            Board.Clear();
            for (int r = 0; r < 3; r++)
            {
                var row = new ObservableCollection<string>();
                for (int c = 0; c < 3; c++)
                    row.Add(".");
                Board.Add(row);
            }
        }

        private void ApplyBoard(string[][] board)
        {
            if (board == null) return;

            var newBoard = new ObservableCollection<ObservableCollection<string>>();
            for (int r = 0; r < 3 && r < board.Length; r++)
            {
                var row = new ObservableCollection<string>();
                for (int c = 0; c < 3 && c < board[r].Length; c++)
                    row.Add(board[r][c] ?? ".");
                newBoard.Add(row);
            }
            if (newBoard.Count == 3)
                Board = newBoard;
        }

        private void UpdateStatusMessage()
        {
            if (Winner != "." && Winner != "Forfeit")
            {
                if (Winner == "Draw")
                    StatusMessage = "It's a draw!";
                else if (Winner == _myMark)
                    StatusMessage = "You win!";
                else
                    StatusMessage = $"{OpponentName ?? "Opponent"} wins!";
            }
            else if (Winner == "Forfeit")
            {
                // Already set by OnGameEndedReceived
            }
            else if (IsMyTurn)
            {
                StatusMessage = $"Your turn ({_myMark})";
            }
            else
            {
                StatusMessage = $"Waiting for {OpponentName ?? "opponent"}...";
            }
        }

        private bool CanClickCell(string parameter)
        {
            if (string.IsNullOrEmpty(parameter) || Winner != "." || IsBusy || !IsMyTurn)
                return false;

            var parts = parameter.Split(',');
            if (parts.Length != 2) return false;
            if (!int.TryParse(parts[0].Trim(), out int row) || !int.TryParse(parts[1].Trim(), out int col))
                return false;
            if (row < 1 || row > 3 || col < 1 || col > 3) return false;
            
            // Check if cell is empty
            if (Board != null && row <= Board.Count && col <= Board[row - 1].Count)
            {
                return Board[row - 1][col - 1] == ".";
            }
            return false;
        }

        private async Task CellClickFromParameterAsync(string parameter)
        {
            if (!CanClickCell(parameter)) return;

            var parts = parameter.Split(',');
            int.TryParse(parts[0].Trim(), out int row);
            int.TryParse(parts[1].Trim(), out int col);

            var command = new PutSquareCommand
            {
                GameId = _gameId,
                PlayerId = _myPlayerId,
                Row = row,
                Column = col
            };

            var result = await ExecuteCommandAsync(command, "Failed to place mark");
            if (result != null)
            {
                if (!result.Success)
                {
                    await DialogService.ShowErrorAsync("Move Failed", result.ErrorMessage);
                }
                // The server will broadcast the update via SignalR
            }
        }

        private async Task ResetAsync()
        {
            var command = new ResetGameCommand
            {
                GameId = _gameId,
                PlayerId = _myPlayerId
            };
            await ExecuteCommandAsync(command, "Failed to reset game");
            // The server will broadcast the update via SignalR
        }

        private async Task LeaveGameAsync()
        {
            if (!string.IsNullOrEmpty(_gameId))
            {
                var command = new LeaveGameCommand
                {
                    GameId = _gameId,
                    PlayerId = _myPlayerId
                };
                await ExecuteCommandAsync(command, "Failed to leave game");
                await SignalR.LeaveGame(_gameId);
            }
            
            GameLeft?.Invoke();
        }

        private void RaiseAllCanExecuteChanged()
        {
            CellClickCommand.RaiseCanExecuteChanged();
            ResetCommand.RaiseCanExecuteChanged();
            LeaveGameCommand.RaiseCanExecuteChanged();
            OnPropertyChanged(nameof(IsGameOver));
        }

        public override async Task OnNavigatedFromAsync()
        {
            SignalR.GameStateUpdateReceived -= OnGameStateUpdateReceived;
            SignalR.GameEndedReceived -= OnGameEndedReceived;

            if (!string.IsNullOrEmpty(_gameId))
            {
                await SignalR.LeaveGame(_gameId);
            }
        }
    }
}
