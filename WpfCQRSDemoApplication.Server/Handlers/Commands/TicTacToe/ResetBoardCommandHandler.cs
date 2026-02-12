using WpfCQRSDemoApplication.Server.Domain.TicTacToe;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Commands.TicTacToe;
using AppLogger = WpfCQRSDemoApplication.Server.Infrastructure.Logging.ILogger;

namespace WpfCQRSDemoApplication.Server.Handlers.Commands.TicTacToe;

public class ResetBoardCommandHandler : ICommandHandler<ResetBoardCommand>
{
    private readonly ITicTacToeBoardStore _store;
    private readonly AppLogger _logger;

    public ResetBoardCommandHandler(ITicTacToeBoardStore store, AppLogger logger)
    {
        _store = store;
        _logger = logger;
    }

    public Task HandleAsync(ResetBoardCommand command)
    {
        _store.Reset();
        _logger.Info("Tic Tac Toe board reset.");
        return Task.CompletedTask;
    }
}
