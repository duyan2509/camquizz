using System.Collections.Concurrent;

namespace CamQuizz.Infrastructure.SignalR;

public class GroupChatConnectionManager
{
    private readonly ConcurrentDictionary<string, Guid> _groupConnections = new();

    public void AddToGroup(Guid groupId, string connectionId)
    {
        _groupConnections[connectionId] = groupId;
    }

    public void RemoveFromGroup(Guid groupId, string connectionId)
    {
        _groupConnections.TryRemove(connectionId, out _);
    }

    public Guid GetGroupOfConnection(string connectionId)
    {
        if (_groupConnections.TryGetValue(connectionId, out var groupId))
            return groupId;
        return Guid.Empty;
    }
}
