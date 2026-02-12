using Microsoft.AspNetCore.SignalR;
using WpfCQRSDemoApplication.Server.Domain.TicTacToe;
using WpfCQRSDemoApplication.Server.Infrastructure.SignalR;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Commands.TicTacToe;
using WpfCQRSDemoApplication.Shared.Infrastructure.SignalR;
using AppLogger = WpfCQRSDemoApplication.Server.Infrastructure.Logging.ILogger;

namespace WpfCQRSDemoApplication.Server.Handlers.Commands.TicTacToe;

public class ResetBoardCommandHandler : ICommandHandler<ResetBoardCommand>
{
    private readonly ITicTacToeBoardStore _store;
    private readonly AppLogger _logger;
    private readonly IHubContext<LiveUpdateHub, ILiveUpdateClient> _hubContext;

    public ResetBoardCommandHandler(
        ITicTacToeBoardStore store,
        AppLogger logger,
        IHubContext<LiveUpdateHub, ILiveUpdateClient> hubContext)
    {
        _store = store;
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task HandleAsync(ResetBoardCommand command)
    {
        _store.Reset();
        _logger.Info("Tic Tac Toe board reset.");
        var status = _store.GetBoard();
        await _hubContext.Clients.Group("game_default").ReceiveGameUpdate("default", status);
    }
}
