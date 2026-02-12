using System;
using System.Collections.Generic;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;
using WpfCQRSDemoApplication.Shared.Contracts.Queries;

namespace WpfCQRSDemoApplication.Shared.DTOs.Lobby
{
    public enum LobbyStatus
    {
        Waiting,
        InGame,
        Closed
    }

    public class LobbyDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string HostPlayerId { get; set; }
        public List<PlayerDto> Players { get; set; } = new List<PlayerDto>();
        public LobbyStatus Status { get; set; }
        public string GameId { get; set; }
    }

    public class PlayerDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsReady { get; set; }
        public string AssignedMark { get; set; } // "X", "O", or null
    }

    public class LobbyListDto
    {
        public List<LobbyInfoDto> Lobbies { get; set; } = new List<LobbyInfoDto>();
    }

    public class LobbyInfoDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int PlayerCount { get; set; }
        public LobbyStatus Status { get; set; }
    }

    public class CreateLobbyResultDto
    {
        public string LobbyId { get; set; }
        public string PlayerId { get; set; }
    }

    public class JoinLobbyResultDto
    {
        public string PlayerId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}

namespace WpfCQRSDemoApplication.Shared.Contracts.Commands.Lobby
{
    public class CreateLobbyCommand : ICommand<WpfCQRSDemoApplication.Shared.DTOs.Lobby.CreateLobbyResultDto>
    {
        public string PlayerName { get; set; }
        public string LobbyName { get; set; }
        public bool ExecuteOnServer => true;
    }

    public class JoinLobbyCommand : ICommand<WpfCQRSDemoApplication.Shared.DTOs.Lobby.JoinLobbyResultDto>
    {
        public string LobbyId { get; set; }
        public string PlayerName { get; set; }
        public bool ExecuteOnServer => true;
    }

    public class LeaveLobbyCommand : ICommand
    {
        public string LobbyId { get; set; }
        public string PlayerId { get; set; }
        public bool ExecuteOnServer => true;
    }

    public class SetPlayerReadyCommand : ICommand
    {
        public string LobbyId { get; set; }
        public string PlayerId { get; set; }
        public bool IsReady { get; set; }
        public bool ExecuteOnServer => true;
    }

    public class StartGameCommand : ICommand<WpfCQRSDemoApplication.Shared.DTOs.Game.GameStartedDto>
    {
        public string LobbyId { get; set; }
        public string PlayerId { get; set; }
        public bool ExecuteOnServer => true;
    }
}

namespace WpfCQRSDemoApplication.Shared.Contracts.Queries.Lobby
{
    public class GetLobbyStatusQuery : IQuery<WpfCQRSDemoApplication.Shared.DTOs.Lobby.LobbyDto>
    {
        public string LobbyId { get; set; }
        public bool ExecuteOnServer => true;
    }

    public class GetLobbyListQuery : IQuery<WpfCQRSDemoApplication.Shared.DTOs.Lobby.LobbyListDto>
    {
        public bool ExecuteOnServer => true;
    }
}
