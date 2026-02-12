using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfCQRSDemo.Helpers;
using WpfCQRSDemo.ViewModels.Base;
using WpfCQRSDemoApplication.Shared.Contracts.Commands.TicTacToe;
using WpfCQRSDemoApplication.Shared.Contracts.Queries;
using WpfCQRSDemoApplication.Shared.DTOs.TicTacToe;
using WpfCQRSDemo.Infrastructure.Services;

namespace WpfCQRSDemo.ViewModels
{
    public class TicTacToeViewModel : ViewModelBase
    {
        private string _winner;
        private string _statusMessage;
        private string _currentPlayer;
        private ObservableCollection<ObservableCollection<string>> _board;

        public TicTacToeViewModel(IInfrastructureServices infrastructure)
            : base(infrastructure)
        {
            _currentPlayer = "X";
            _winner = ".";
            _statusMessage = "Your turn (X)";
            _board = new ObservableCollection<ObservableCollection<string>>();
            for (int r = 0; r < 3; r++)
            {
                var row = new ObservableCollection<string>();
                for (int c = 0; c < 3; c++)
                    row.Add(".");
                _board.Add(row);
            }

            CellClickCommand = new AsyncRelayCommand<string>(CellClickFromParameterAsync, p => Winner == "." && !IsBusy && !string.IsNullOrEmpty(p));
            LoadBoardCommand = new AsyncRelayCommand(LoadBoardAsync);
            ResetCommand = new AsyncRelayCommand(ResetAsync, () => !IsBusy);
        }

        public ObservableCollection<ObservableCollection<string>> Board
        {
            get { return _board; }
            set { SetProperty(ref _board, value); }
        }

        public string Winner
        {
            get { return _winner; }
            set { SetProperty(ref _winner, value); }
        }

        public string StatusMessage
        {
            get { return _statusMessage; }
            set { SetProperty(ref _statusMessage, value); }
        }

        public ICommand CellClickCommand { get; }
        public ICommand LoadBoardCommand { get; }
        public ICommand ResetCommand { get; }

        public override async Task InitializeAsync()
        {
            await LoadBoardAsync();
        }

        private async Task LoadBoardAsync()
        {
            var query = new GetBoardQuery();
            var status = await ExecuteQueryAsync(query, "Failed to load board", "Loading...");
            if (status != null)
                ApplyStatus(status);
        }

        private void ApplyStatus(BoardStatusDto status)
        {
            if (status.Board != null)
            {
                var newBoard = new ObservableCollection<ObservableCollection<string>>();
                for (int r = 0; r < 3 && r < status.Board.Length; r++)
                {
                    var row = new ObservableCollection<string>();
                    for (int c = 0; c < 3 && c < status.Board[r].Length; c++)
                        row.Add(status.Board[r][c] ?? ".");
                    newBoard.Add(row);
                }
                if (newBoard.Count == 3)
                    Board = newBoard;
            }
            Winner = status.Winner ?? ".";
            UpdateStatusMessage();
            (CellClickCommand as AsyncRelayCommand<string>)?.RaiseCanExecuteChanged();
            (ResetCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
        }

        private void UpdateStatusMessage()
        {
            if (Winner != ".")
                StatusMessage = Winner == "X" ? "X wins!" : Winner == "O" ? "O wins!" : "Game over.";
            else
                StatusMessage = "Your turn (" + _currentPlayer + ")";
        }

        private async Task CellClickFromParameterAsync(string parameter)
        {
            if (string.IsNullOrEmpty(parameter) || Winner != ".") return;
            var parts = parameter.Split(',');
            if (parts.Length != 2) return;
            if (!int.TryParse(parts[0].Trim(), out int row) || !int.TryParse(parts[1].Trim(), out int col))
                return;
            if (row < 1 || row > 3 || col < 1 || col > 3) return;
            if (Board != null && row <= Board.Count && col <= Board[row - 1].Count && Board[row - 1][col - 1] != ".")
                return;

            var command = new PutSquareCommand
            {
                Row = row,
                Column = col,
                Mark = _currentPlayer
            };
            var status = await ExecuteCommandAsync(command, "Failed to place mark");
            if (status != null)
            {
                ApplyStatus(status);
                _currentPlayer = _currentPlayer == "X" ? "O" : "X";
                UpdateStatusMessage();
            }
        }

        private async Task ResetAsync()
        {
            var command = new ResetBoardCommand();
            await ExecuteCommandAsync(command, "Failed to reset board");
            _currentPlayer = "X";
            await LoadBoardAsync();
        }
    }
}
