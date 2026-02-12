using System.Collections.Generic;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Queries;

namespace WpfCQRSDemoApplication.Shared.DTOs.Game
{
    public enum GameStatus
    {
        InProgress,
        Finished
    }

    public class GameStateDto
    {
        public string Id { get; set; }
        public string LobbyId { get; set; }
        public GamePlayerDto PlayerX { get; set; }
        public GamePlayerDto PlayerO { get; set; }
        public string CurrentTurnPlayerId { get; set; }
        public string[][] Board { get; set; }
        public string Winner { get; set; } // ".", "X", "O", or "Draw"
        public GameStatus Status { get; set; }
    }

    public class GamePlayerDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Mark { get; set; } // "X" or "O"
    }

    public class GameStartedDto
    {
        public string GameId { get; set; }
        public string YourPlayerId { get; set; }
        public string YourMark { get; set; } // "X" or "O"
        public bool IsYourTurn { get; set; }
        public string OpponentName { get; set; }
        public string[][] Board { get; set; }
    }

    public class GameUpdateDto
    {
        public string GameId { get; set; }
        public string[][] Board { get; set; }
        public string CurrentTurnPlayerId { get; set; }
        public string Winner { get; set; } // ".", "X", "O", or "Draw"
        public GameStatus Status { get; set; }
        public string LastMovePlayerId { get; set; }
        public int LastMoveRow { get; set; }
        public int LastMoveColumn { get; set; }
    }

    public class PutSquareResultDto
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string[][] Board { get; set; }
        public string Winner { get; set; }
        public string NextTurnPlayerId { get; set; }
    }
}

namespace WpfCQRSDemoApplication.Shared.Contracts.Commands.Game
{
    public class PutSquareCommand : ICommand<WpfCQRSDemoApplication.Shared.DTOs.Game.PutSquareResultDto>
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        // Mark is derived from player assignment, not sent by client
        public bool ExecuteOnServer => true;
    }

    public class ResetGameCommand : ICommand
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
        public bool ExecuteOnServer => true;
    }

    public class LeaveGameCommand : ICommand
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
        public bool ExecuteOnServer => true;
    }
}

namespace WpfCQRSDemoApplication.Shared.Contracts.Queries.Game
{
    public class GetGameStateQuery : IQuery<WpfCQRSDemoApplication.Shared.DTOs.Game.GameStateDto>
    {
        public string GameId { get; set; }
        public bool ExecuteOnServer => true;
    }
}
