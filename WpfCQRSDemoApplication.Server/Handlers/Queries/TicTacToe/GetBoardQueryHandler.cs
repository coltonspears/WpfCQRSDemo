using WpfCQRSDemoApplication.Server.Domain.TicTacToe;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Queries;
using WpfCQRSDemoApplication.Shared.DTOs.TicTacToe;

namespace WpfCQRSDemoApplication.Server.Handlers.Queries.TicTacToe;

public class GetBoardQueryHandler : IQueryHandler<GetBoardQuery, BoardStatusDto>
{
    private readonly ITicTacToeBoardStore _store;

    public GetBoardQueryHandler(ITicTacToeBoardStore store)
    {
        _store = store;
    }

    public Task<BoardStatusDto> HandleAsync(GetBoardQuery query)
    {
        return Task.FromResult(_store.GetBoard());
    }
}
