using Microsoft.AspNetCore.SignalR;
using WpfCQRSDemoApplication.Server.Domain.TicTacToe;
using WpfCQRSDemoApplication.Server.Infrastructure.SignalR;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Commands.Game;
using WpfCQRSDemoApplication.Shared.Contracts.Queries.Game;
using WpfCQRSDemoApplication.Shared.DTOs.Game;
using WpfCQRSDemoApplication.Shared.Infrastructure.SignalR;
using AppLogger = WpfCQRSDemoApplication.Server.Infrastructure.Logging.ILogger;

namespace WpfCQRSDemoApplication.Server.Handlers.Game;

public class GameHandlers :
    ICommandHandler<PutSquareCommand, PutSquareResultDto>,
    ICommandHandler<ResetGameCommand>,
    ICommandHandler<LeaveGameCommand>,
    IQueryHandler<GetGameStateQuery, GameStateDto>
{
    private readonly IGameStore _gameStore;
    private readonly AppLogger _logger;
    private readonly IHubContext<LiveUpdateHub, ILiveUpdateClient> _hubContext;

    public GameHandlers(
        IGameStore gameStore,
        AppLogger logger,
        IHubContext<LiveUpdateHub, ILiveUpdateClient> hubContext)
    {
        _gameStore = gameStore;
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task<PutSquareResultDto> HandleAsync(PutSquareCommand command)
    {
        var result = _gameStore.MakeMove(command.GameId, command.PlayerId, command.Row, command.Column);

        if (result.Success)
        {
            _logger.Info($"Player {command.PlayerId} placed mark at ({command.Row},{command.Column}) in game {command.GameId}");

            var game = _gameStore.GetGame(command.GameId);
            
            // Broadcast game update to all players in the game
            var gameUpdate = new GameUpdateDto
            {
                GameId = command.GameId,
                Board = result.Board,
                CurrentTurnPlayerId = result.NextTurnPlayerId,
                Winner = result.Winner,
                Status = game?.Status ?? GameStatus.Finished,
                LastMovePlayerId = command.PlayerId,
                LastMoveRow = command.Row,
                LastMoveColumn = command.Column
            };
            
            await _hubContext.Clients.Group($"game_{command.GameId}").ReceiveGameStateUpdate(gameUpdate);

            // If game ended, notify about game end
            if (result.Winner != ".")
            {
                string winnerId = null;
                string winnerName = null;
                
                if (result.Winner == "X")
                {
                    winnerId = game?.PlayerX?.Id;
                    winnerName = game?.PlayerX?.Name;
                }
                else if (result.Winner == "O")
                {
                    winnerId = game?.PlayerO?.Id;
                    winnerName = game?.PlayerO?.Name;
                }
                else if (result.Winner == "Draw")
                {
                    winnerName = "Draw";
                }

                await _hubContext.Clients.Group($"game_{command.GameId}")
                    .ReceiveGameEnded(command.GameId, winnerId, winnerName);
            }
        }
        else
        {
            _logger.Info($"Move rejected for player {command.PlayerId}: {result.ErrorMessage}");
        }

        return result;
    }

    public async Task HandleAsync(ResetGameCommand command)
    {
        _gameStore.ResetGame(command.GameId);
        _logger.Info($"Game {command.GameId} reset by player {command.PlayerId}");

        var game = _gameStore.GetGame(command.GameId);
        if (game != null)
        {
            var gameUpdate = new GameUpdateDto
            {
                GameId = command.GameId,
                Board = game.Board,
                CurrentTurnPlayerId = game.CurrentTurnPlayerId,
                Winner = game.Winner,
                Status = game.Status,
                LastMovePlayerId = null,
                LastMoveRow = 0,
                LastMoveColumn = 0
            };
            
            await _hubContext.Clients.Group($"game_{command.GameId}").ReceiveGameStateUpdate(gameUpdate);
        }
    }

    public async Task HandleAsync(LeaveGameCommand command)
    {
        var game = _gameStore.GetGame(command.GameId);
        if (game != null)
        {
            // Determine the other player as winner (forfeit)
            string winnerId = null;
            string winnerName = null;
            
            if (game.PlayerX?.Id == command.PlayerId)
            {
                winnerId = game.PlayerO?.Id;
                winnerName = game.PlayerO?.Name;
            }
            else if (game.PlayerO?.Id == command.PlayerId)
            {
                winnerId = game.PlayerX?.Id;
                winnerName = game.PlayerX?.Name;
            }

            _logger.Info($"Player {command.PlayerId} left game {command.GameId}. Winner: {winnerName ?? "None"}");

            await _hubContext.Clients.Group($"game_{command.GameId}")
                .ReceiveGameEnded(command.GameId, winnerId, $"{winnerName} (opponent left)");
        }

        _gameStore.TryRemoveGame(command.GameId);
    }

    public Task<GameStateDto> HandleAsync(GetGameStateQuery query)
    {
        return Task.FromResult(_gameStore.GetGame(query.GameId));
    }
}
