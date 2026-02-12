using WpfCQRSDemoApplication.Shared.DTOs.TicTacToe;

namespace WpfCQRSDemoApplication.Shared.Contracts.Commands.TicTacToe
{
    public class PutSquareCommand : ICommand<BoardStatusDto>
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public string Mark { get; set; }
        public bool ExecuteOnServer => true;
    }
}
