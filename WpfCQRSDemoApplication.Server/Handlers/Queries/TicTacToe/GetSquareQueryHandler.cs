using System;
using WpfCQRSDemoApplication.Server.Domain.TicTacToe;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Queries;

namespace WpfCQRSDemoApplication.Server.Handlers.Queries.TicTacToe;

public class GetSquareQueryHandler : IQueryHandler<GetSquareQuery, string>
{
    private readonly ITicTacToeBoardStore _store;

    public GetSquareQueryHandler(ITicTacToeBoardStore store)
    {
        _store = store;
    }

    public Task<string> HandleAsync(GetSquareQuery query)
    {
        try
        {
            var mark = _store.GetSquare(query.Row, query.Column);
            return Task.FromResult(mark);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new InvalidOperationException("Illegal coordinates");
        }
    }
}
