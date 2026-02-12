using System;
using WpfCQRSDemoApplication.Server.Domain.TicTacToe;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Commands.TicTacToe;
using WpfCQRSDemoApplication.Shared.DTOs.TicTacToe;
using AppLogger = WpfCQRSDemoApplication.Server.Infrastructure.Logging.ILogger;

namespace WpfCQRSDemoApplication.Server.Handlers.Commands.TicTacToe;

public class PutSquareCommandHandler : ICommandHandler<PutSquareCommand, BoardStatusDto>
{
    private readonly ITicTacToeBoardStore _store;
    private readonly AppLogger _logger;

    public PutSquareCommandHandler(ITicTacToeBoardStore store, AppLogger logger)
    {
        _store = store;
        _logger = logger;
    }

    public Task<BoardStatusDto> HandleAsync(PutSquareCommand command)
    {
        if (command.Mark != "X" && command.Mark != "O")
            throw new InvalidOperationException("Invalid Mark (X or O).");

        try
        {
            var status = _store.SetSquare(command.Row, command.Column, command.Mark);
            _logger.Info($"Put {command.Mark} at ({command.Row},{command.Column})");
            return Task.FromResult(status);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new InvalidOperationException("Illegal coordinates.");
        }
    }
}
