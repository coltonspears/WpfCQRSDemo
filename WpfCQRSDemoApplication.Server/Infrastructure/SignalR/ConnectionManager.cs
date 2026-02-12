using System.Collections.Concurrent;

namespace WpfCQRSDemoApplication.Server.Infrastructure.SignalR;

public class PlayerConnection
{
    public string ConnectionId { get; set; }
    public string PlayerId { get; set; }
    public string PlayerName { get; set; }
    public string CurrentLobbyId { get; set; }
    public string CurrentGameId { get; set; }
}

public interface IConnectionManager
{
    void OnConnected(string connectionId);
    void OnDisconnected(string connectionId);
    PlayerConnection GetConnection(string connectionId);
    PlayerConnection GetConnectionByPlayerId(string playerId);
    void SetPlayerInfo(string connectionId, string playerId, string playerName);
    void SetLobbyId(string connectionId, string lobbyId);
    void SetGameId(string connectionId, string gameId);
    void ClearLobbyId(string connectionId);
    void ClearGameId(string connectionId);
    IEnumerable<PlayerConnection> GetAllConnections();
}

public class ConnectionManager : IConnectionManager
{
    private readonly ConcurrentDictionary<string, PlayerConnection> _connections = new();
    private readonly ConcurrentDictionary<string, string> _playerIdToConnectionId = new();

    public void OnConnected(string connectionId)
    {
        var connection = new PlayerConnection
        {
            ConnectionId = connectionId,
            PlayerId = null,
            PlayerName = null,
            CurrentLobbyId = null,
            CurrentGameId = null
        };
        _connections.TryAdd(connectionId, connection);
    }

    public void OnDisconnected(string connectionId)
    {
        if (_connections.TryRemove(connectionId, out var connection))
        {
            if (!string.IsNullOrEmpty(connection.PlayerId))
            {
                _playerIdToConnectionId.TryRemove(connection.PlayerId, out _);
            }
        }
    }

    public PlayerConnection GetConnection(string connectionId)
    {
        _connections.TryGetValue(connectionId, out var connection);
        return connection;
    }

    public PlayerConnection GetConnectionByPlayerId(string playerId)
    {
        if (_playerIdToConnectionId.TryGetValue(playerId, out var connectionId))
        {
            return GetConnection(connectionId);
        }
        return null;
    }

    public void SetPlayerInfo(string connectionId, string playerId, string playerName)
    {
        if (_connections.TryGetValue(connectionId, out var connection))
        {
            // Remove old player ID mapping if exists
            if (!string.IsNullOrEmpty(connection.PlayerId))
            {
                _playerIdToConnectionId.TryRemove(connection.PlayerId, out _);
            }

            connection.PlayerId = playerId;
            connection.PlayerName = playerName;

            if (!string.IsNullOrEmpty(playerId))
            {
                _playerIdToConnectionId.TryAdd(playerId, connectionId);
            }
        }
    }

    public void SetLobbyId(string connectionId, string lobbyId)
    {
        if (_connections.TryGetValue(connectionId, out var connection))
        {
            connection.CurrentLobbyId = lobbyId;
        }
    }

    public void SetGameId(string connectionId, string gameId)
    {
        if (_connections.TryGetValue(connectionId, out var connection))
        {
            connection.CurrentGameId = gameId;
        }
    }

    public void ClearLobbyId(string connectionId)
    {
        if (_connections.TryGetValue(connectionId, out var connection))
        {
            connection.CurrentLobbyId = null;
        }
    }

    public void ClearGameId(string connectionId)
    {
        if (_connections.TryGetValue(connectionId, out var connection))
        {
            connection.CurrentGameId = null;
        }
    }

    public IEnumerable<PlayerConnection> GetAllConnections()
    {
        return _connections.Values;
    }
}
