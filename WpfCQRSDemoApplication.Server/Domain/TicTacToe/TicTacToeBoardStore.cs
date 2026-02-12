using System;
using System.Linq;
using WpfCQRSDemoApplication.Shared.DTOs.TicTacToe;

namespace WpfCQRSDemoApplication.Server.Domain.TicTacToe;

public class TicTacToeBoardStore : ITicTacToeBoardStore
{
    private readonly object _lock = new object();
    private readonly string[][] _board = new[]
    {
        new[] { ".", ".", "." },
        new[] { ".", ".", "." },
        new[] { ".", ".", "." }
    };

    public BoardStatusDto GetBoard()
    {
        lock (_lock)
        {
            var boardCopy = _board.Select(r => r.ToArray()).ToArray();
            return new BoardStatusDto
            {
                Winner = ComputeWinner(_board),
                Board = boardCopy
            };
        }
    }

    public string GetSquare(int row, int column)
    {
        var r = ToZeroBased(row);
        var c = ToZeroBased(column);
        lock (_lock)
        {
            return _board[r][c];
        }
    }

    public BoardStatusDto SetSquare(int row, int column, string mark)
    {
        var r = ToZeroBased(row);
        var c = ToZeroBased(column);
        lock (_lock)
        {
            if (_board[r][c] != ".")
                throw new InvalidOperationException("Square is not empty.");
            _board[r][c] = mark;
            var boardCopy = _board.Select(arr => arr.ToArray()).ToArray();
            return new BoardStatusDto
            {
                Winner = ComputeWinner(_board),
                Board = boardCopy
            };
        }
    }

    public void Reset()
    {
        lock (_lock)
        {
            for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
                _board[i][j] = ".";
        }
    }

    private static int ToZeroBased(int oneBased)
    {
        if (oneBased < 1 || oneBased > 3)
            throw new ArgumentOutOfRangeException(nameof(oneBased), "Coordinates must be 1-3.");
        return oneBased - 1;
    }

    private static string ComputeWinner(string[][] board)
    {
        for (var i = 0; i < 3; i++)
        {
            if (board[i][0] != "." && board[i][0] == board[i][1] && board[i][1] == board[i][2])
                return board[i][0];
            if (board[0][i] != "." && board[0][i] == board[1][i] && board[1][i] == board[2][i])
                return board[0][i];
        }
        if (board[0][0] != "." && board[0][0] == board[1][1] && board[1][1] == board[2][2])
            return board[0][0];
        if (board[0][2] != "." && board[0][2] == board[1][1] && board[1][1] == board[2][0])
            return board[0][2];
        return ".";
    }
}
