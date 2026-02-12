using WpfCQRSDemoApplication.Shared.Contracts.Commands;

namespace WpfCQRSDemoApplication.Shared.Contracts.Commands.TicTacToe
{
    public class ResetBoardCommand : ICommand
    {
        public bool ExecuteOnServer => true;
    }
}
