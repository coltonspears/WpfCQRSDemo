using System;
using Microsoft.AspNetCore.SignalR;
using WpfCQRSDemoApplication.Server.Domain.TicTacToe;
using WpfCQRSDemoApplication.Server.Infrastructure.SignalR;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Commands.TicTacToe;
using WpfCQRSDemoApplication.Shared.Infrastructure.SignalR;
using WpfCQRSDemoApplication.Shared.DTOs.TicTacToe;
using AppLogger = WpfCQRSDemoApplication.Server.Infrastructure.Logging.ILogger;

namespace WpfCQRSDemoApplication.Server.Handlers.Commands.TicTacToe;

public class PutSquareCommandHandler : ICommandHandler<PutSquareCommand, BoardStatusDto>
{
    private readonly ITicTacToeBoardStore _store;
    private readonly AppLogger _logger;
    private readonly IHubContext<LiveUpdateHub, ILiveUpdateClient> _hubContext;

    public PutSquareCommandHandler(
        ITicTacToeBoardStore store,
        AppLogger logger,
        IHubContext<LiveUpdateHub, ILiveUpdateClient> hubContext)
    {
        _store = store;
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task<BoardStatusDto> HandleAsync(PutSquareCommand command)
    {
        if (command.Mark != "X" && command.Mark != "O")
            throw new InvalidOperationException("Invalid Mark (X or O).");

        try
        {
            var status = _store.SetSquare(command.Row, command.Column, command.Mark);
            _logger.Info($"Put {command.Mark} at ({command.Row},{command.Column})");
            await _hubContext.Clients.Group("game_default").ReceiveGameUpdate("default", status);
            return status;
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new InvalidOperationException("Illegal coordinates.");
        }
    }
}
