using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.DTOs.TicTacToe;

namespace WpfCQRSDemoApplication.Shared.Contracts.Queries
{
    public class GetBoardQuery : IQuery<BoardStatusDto>
    {
        public bool ExecuteOnServer => true;
    }
}
