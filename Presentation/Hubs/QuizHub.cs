using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace CamQuizz.Presentation.Hubs;

public class QuizHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        await Clients.Caller.SendAsync("Connected", "Connected to QuizHub");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinQuizGroup(string quizId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Quiz_{quizId}");
        await Clients.Group($"Quiz_{quizId}").SendAsync("UserJoinedQuiz", Context.UserIdentifier, quizId);
    }

    public async Task LeaveQuizGroup(string quizId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Quiz_{quizId}");
        await Clients.Group($"Quiz_{quizId}").SendAsync("UserLeftQuiz", Context.UserIdentifier, quizId);
    }

    public async Task SendQuizUpdate(string quizId, string message)
    {
        await Clients.Group($"Quiz_{quizId}").SendAsync("QuizUpdated", message, quizId);
    }

    public async Task SendNotification(string userId, string message)
    {
        await Clients.User(userId).SendAsync("Notification", message);
    }
} 