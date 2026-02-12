using System;
using System.Collections.Concurrent;
using System.Linq;
using WpfCQRSDemoApplication.Shared.DTOs.Game;
using WpfCQRSDemoApplication.Shared.DTOs.Lobby;

namespace WpfCQRSDemoApplication.Server.Domain.TicTacToe;

public interface IGameStore
{
    GameStateDto CreateGame(string lobbyId, PlayerDto playerX, PlayerDto playerO);
    GameStateDto GetGame(string gameId);
    PutSquareResultDto MakeMove(string gameId, string playerId, int row, int column);
    void ResetGame(string gameId);
    bool TryRemoveGame(string gameId);
    string GetGameIdForPlayer(string playerId);
    bool IsPlayerTurn(string gameId, string playerId);
    string GetPlayerMark(string gameId, string playerId);
}

public class GameStore : IGameStore
{
    private readonly ConcurrentDictionary<string, GameStateDto> _games = new();
    private readonly ConcurrentDictionary<string, string> _playerToGame = new(); // PlayerId -> GameId
    private readonly object _lock = new();

    public GameStateDto CreateGame(string lobbyId, PlayerDto playerX, PlayerDto playerO)
    {
        var gameId = Guid.NewGuid().ToString("N")[..8];

        var game = new GameStateDto
        {
            Id = gameId,
            LobbyId = lobbyId,
            PlayerX = new GamePlayerDto
            {
                Id = playerX.Id,
                Name = playerX.Name,
                Mark = "X"
            },
            PlayerO = new GamePlayerDto
            {
                Id = playerO.Id,
                Name = playerO.Name,
                Mark = "O"
            },
            CurrentTurnPlayerId = playerX.Id, // X always goes first
            Board = CreateEmptyBoard(),
            Winner = ".",
            Status = GameStatus.InProgress
        };

        _games.TryAdd(gameId, game);
        _playerToGame.TryAdd(playerX.Id, gameId);
        _playerToGame.TryAdd(playerO.Id, gameId);

        return game;
    }

    public GameStateDto GetGame(string gameId)
    {
        _games.TryGetValue(gameId, out var game);
        return game;
    }

    public PutSquareResultDto MakeMove(string gameId, string playerId, int row, int column)
    {
        lock (_lock)
        {
            if (!_games.TryGetValue(gameId, out var game))
            {
                return new PutSquareResultDto
                {
                    Success = false,
                    ErrorMessage = "Game not found."
                };
            }

            if (game.Status != GameStatus.InProgress)
            {
                return new PutSquareResultDto
                {
                    Success = false,
                    ErrorMessage = "Game has already ended."
                };
            }

            if (game.CurrentTurnPlayerId != playerId)
            {
                return new PutSquareResultDto
                {
                    Success = false,
                    ErrorMessage = "It's not your turn."
                };
            }

            // Validate player is in this game
            var mark = GetPlayerMarkInternal(game, playerId);
            if (mark == null)
            {
                return new PutSquareResultDto
                {
                    Success = false,
                    ErrorMessage = "Player is not part of this game."
                };
            }

            // Validate coordinates
            var r = ToZeroBased(row);
            var c = ToZeroBased(column);

            if (r < 0 || r > 2 || c < 0 || c > 2)
            {
                return new PutSquareResultDto
                {
                    Success = false,
                    ErrorMessage = "Invalid coordinates. Must be 1-3."
                };
            }

            // Check if square is empty
            if (game.Board[r][c] != ".")
            {
                return new PutSquareResultDto
                {
                    Success = false,
                    ErrorMessage = "Square is not empty."
                };
            }

            // Make the move
            game.Board[r][c] = mark;

            // Check for winner
            var winner = ComputeWinner(game.Board);
            game.Winner = winner;

            // Check for draw
            if (winner == "." && IsBoardFull(game.Board))
            {
                game.Winner = "Draw";
                game.Status = GameStatus.Finished;
            }
            else if (winner != ".")
            {
                game.Status = GameStatus.Finished;
            }

            // Switch turn
            var nextPlayerId = game.CurrentTurnPlayerId == game.PlayerX.Id
                ? game.PlayerO.Id
                : game.PlayerX.Id;
            game.CurrentTurnPlayerId = nextPlayerId;

            return new PutSquareResultDto
            {
                Success = true,
                Board = game.Board.Select(arr => arr.ToArray()).ToArray(),
                Winner = game.Winner,
                NextTurnPlayerId = game.Status == GameStatus.Finished ? null : nextPlayerId
            };
        }
    }

    public void ResetGame(string gameId)
    {
        lock (_lock)
        {
            if (_games.TryGetValue(gameId, out var game))
            {
                game.Board = CreateEmptyBoard();
                game.Winner = ".";
                game.Status = GameStatus.InProgress;
                game.CurrentTurnPlayerId = game.PlayerX.Id; // X always starts
            }
        }
    }

    public bool TryRemoveGame(string gameId)
    {
        if (_games.TryRemove(gameId, out var game))
        {
            _playerToGame.TryRemove(game.PlayerX.Id, out _);
            _playerToGame.TryRemove(game.PlayerO.Id, out _);
            return true;
        }
        return false;
    }

    public string GetGameIdForPlayer(string playerId)
    {
        _playerToGame.TryGetValue(playerId, out var gameId);
        return gameId;
    }

    public bool IsPlayerTurn(string gameId, string playerId)
    {
        if (_games.TryGetValue(gameId, out var game))
        {
            return game.CurrentTurnPlayerId == playerId;
        }
        return false;
    }

    public string GetPlayerMark(string gameId, string playerId)
    {
        if (_games.TryGetValue(gameId, out var game))
        {
            return GetPlayerMarkInternal(game, playerId);
        }
        return null;
    }

    private static string GetPlayerMarkInternal(GameStateDto game, string playerId)
    {
        if (game.PlayerX?.Id == playerId) return "X";
        if (game.PlayerO?.Id == playerId) return "O";
        return null;
    }

    private static string[][] CreateEmptyBoard()
    {
        return new[]
        {
            new[] { ".", ".", "." },
            new[] { ".", ".", "." },
            new[] { ".", ".", "." }
        };
    }

    private static int ToZeroBased(int oneBased)
    {
        return oneBased - 1;
    }

    private static bool IsBoardFull(string[][] board)
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                if (board[i][j] == ".") return false;
            }
        }
        return true;
    }

    private static string ComputeWinner(string[][] board)
    {
        // Check rows
        for (var i = 0; i < 3; i++)
        {
            if (board[i][0] != "." && board[i][0] == board[i][1] && board[i][1] == board[i][2])
                return board[i][0];
        }

        // Check columns
        for (var i = 0; i < 3; i++)
        {
            if (board[0][i] != "." && board[0][i] == board[1][i] && board[1][i] == board[2][i])
                return board[0][i];
        }

        // Check diagonals
        if (board[0][0] != "." && board[0][0] == board[1][1] && board[1][1] == board[2][2])
            return board[0][0];
        if (board[0][2] != "." && board[0][2] == board[1][1] && board[1][1] == board[2][0])
            return board[0][2];

        return ".";
    }
}
