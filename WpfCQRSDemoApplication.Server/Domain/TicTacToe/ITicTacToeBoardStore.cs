using WpfCQRSDemoApplication.Shared.DTOs.TicTacToe;

namespace WpfCQRSDemoApplication.Server.Domain.TicTacToe;

public interface ITicTacToeBoardStore
{
    BoardStatusDto GetBoard();
    string GetSquare(int row, int column);
    BoardStatusDto SetSquare(int row, int column, string mark);
    void Reset();
}
