using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WpfCQRSDemoApplication.Shared.DTOs.Lobby;

namespace WpfCQRSDemoApplication.Server.Domain.Lobby;

public interface ILobbyStore
{
    // Lobby management
    LobbyDto CreateLobby(string lobbyName, PlayerDto hostPlayer);
    LobbyDto GetLobby(string lobbyId);
    LobbyListDto GetLobbyList();
    bool TryRemoveLobby(string lobbyId);

    // Player management within lobbies
    bool TryJoinLobby(string lobbyId, PlayerDto player, out string errorMessage);
    bool TryLeaveLobby(string lobbyId, string playerId);
    void SetPlayerReady(string lobbyId, string playerId, bool isReady);
    void AssignPlayerMark(string lobbyId, string playerId, string mark);

    // Game association
    void SetLobbyGameId(string lobbyId, string gameId);
    void SetLobbyStatus(string lobbyId, LobbyStatus status);

    // Connection tracking helpers
    string GetLobbyIdForPlayer(string playerId);
}

public class LobbyStore : ILobbyStore
{
    private readonly ConcurrentDictionary<string, LobbyDto> _lobbies = new();
    private readonly ConcurrentDictionary<string, string> _playerToLobby = new(); // PlayerId -> LobbyId
    private readonly object _lock = new();

    public LobbyDto CreateLobby(string lobbyName, PlayerDto hostPlayer)
    {
        var lobbyId = Guid.NewGuid().ToString("N")[..8]; // Short unique ID
        var lobby = new LobbyDto
        {
            Id = lobbyId,
            Name = lobbyName,
            HostPlayerId = hostPlayer.Id,
            Players = new List<PlayerDto> { hostPlayer },
            Status = LobbyStatus.Waiting,
            GameId = null
        };

        _lobbies.TryAdd(lobbyId, lobby);
        _playerToLobby.TryAdd(hostPlayer.Id, lobbyId);

        return lobby;
    }

    public LobbyDto GetLobby(string lobbyId)
    {
        _lobbies.TryGetValue(lobbyId, out var lobby);
        return lobby;
    }

    public LobbyListDto GetLobbyList()
    {
        var lobbies = _lobbies.Values
            .Where(l => l.Status == LobbyStatus.Waiting)
            .Select(l => new LobbyInfoDto
            {
                Id = l.Id,
                Name = l.Name,
                PlayerCount = l.Players.Count,
                Status = l.Status
            })
            .ToList();

        return new LobbyListDto { Lobbies = lobbies };
    }

    public bool TryRemoveLobby(string lobbyId)
    {
        if (_lobbies.TryRemove(lobbyId, out var lobby))
        {
            foreach (var player in lobby.Players)
            {
                _playerToLobby.TryRemove(player.Id, out _);
            }
            return true;
        }
        return false;
    }

    public bool TryJoinLobby(string lobbyId, PlayerDto player, out string errorMessage)
    {
        lock (_lock)
        {
            if (!_lobbies.TryGetValue(lobbyId, out var lobby))
            {
                errorMessage = "Lobby not found.";
                return false;
            }

            if (lobby.Status != LobbyStatus.Waiting)
            {
                errorMessage = "Lobby is no longer accepting players.";
                return false;
            }

            if (lobby.Players.Count >= 2)
            {
                errorMessage = "Lobby is full.";
                return false;
            }

            if (lobby.Players.Any(p => p.Id == player.Id))
            {
                errorMessage = "Player is already in this lobby.";
                return false;
            }

            lobby.Players.Add(player);
            _playerToLobby.TryAdd(player.Id, lobbyId);
            errorMessage = null;
            return true;
        }
    }

    public bool TryLeaveLobby(string lobbyId, string playerId)
    {
        lock (_lock)
        {
            if (!_lobbies.TryGetValue(lobbyId, out var lobby))
            {
                return false;
            }

            var player = lobby.Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null)
            {
                return false;
            }

            lobby.Players.Remove(player);
            _playerToLobby.TryRemove(playerId, out _);

            // If lobby is empty, remove it
            if (lobby.Players.Count == 0)
            {
                _lobbies.TryRemove(lobbyId, out _);
            }
            // If host left and there are other players, assign new host
            else if (lobby.HostPlayerId == playerId && lobby.Players.Count > 0)
            {
                lobby.HostPlayerId = lobby.Players[0].Id;
            }

            return true;
        }
    }

    public void SetPlayerReady(string lobbyId, string playerId, bool isReady)
    {
        if (_lobbies.TryGetValue(lobbyId, out var lobby))
        {
            var player = lobby.Players.FirstOrDefault(p => p.Id == playerId);
            if (player != null)
            {
                player.IsReady = isReady;
            }
        }
    }

    public void AssignPlayerMark(string lobbyId, string playerId, string mark)
    {
        if (_lobbies.TryGetValue(lobbyId, out var lobby))
        {
            var player = lobby.Players.FirstOrDefault(p => p.Id == playerId);
            if (player != null)
            {
                player.AssignedMark = mark;
            }
        }
    }

    public void SetLobbyGameId(string lobbyId, string gameId)
    {
        if (_lobbies.TryGetValue(lobbyId, out var lobby))
        {
            lobby.GameId = gameId;
        }
    }

    public void SetLobbyStatus(string lobbyId, LobbyStatus status)
    {
        if (_lobbies.TryGetValue(lobbyId, out var lobby))
        {
            lobby.Status = status;
        }
    }

    public string GetLobbyIdForPlayer(string playerId)
    {
        _playerToLobby.TryGetValue(playerId, out var lobbyId);
        return lobbyId;
    }
}
