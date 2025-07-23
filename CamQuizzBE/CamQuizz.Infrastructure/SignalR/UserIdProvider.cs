using Microsoft.AspNetCore.SignalR;

namespace CamQuizz.Infrastructure.SignalR;
public class UserIdProvider : IUserIdProvider 
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst("userId")?.Value;
    }
}


