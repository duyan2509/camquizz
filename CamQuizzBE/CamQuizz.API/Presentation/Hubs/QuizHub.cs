using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.RegularExpressions;
using CamQuizz.Application.Dtos;
using CamQuizz.Application.Exceptions;
using CamQuizz.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace CamQuizz.Presentation.Hubs;
[Authorize]
public class QuizHub : Hub
{
    private readonly IGameService gameService;

    public QuizHub(IGameService gameService)
    {
        this.gameService = gameService;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        await Clients.Caller.SendAsync("Connected", "Connected to QuizHub");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public async Task CreateRoom(Guid quizId)
    {
        try
        {
            var userId = GetUserId();
            var successMessage = await gameService.CreateRoomAsync(Context.ConnectionId, userId, quizId);
            await Groups.AddToGroupAsync(Context.ConnectionId, successMessage.RoomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, quizId.ToString()); // delete when start game
            await Clients.Caller.SendAsync("CreateRoomSuccess", successMessage);
        }
        catch (UniqueRoomException ex)
        {
            await Clients.Caller.SendAsync("CreateRoomError", ex.Message);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("CreateRoomError", ex.Message);
        }
    }
    public async Task JoinRoom(string code)
    {
        try
        {
            var userId = GetUserId();
            var successMessage = await gameService.JoinRoomAsync(Context.ConnectionId, userId, code);
            await Groups.AddToGroupAsync(Context.ConnectionId, successMessage.CallerMessage.RoomId);
            await Clients.Caller.SendAsync("JoinRoomSuccess", successMessage.CallerMessage);
            await Clients.OthersInGroup(successMessage.CallerMessage.RoomId).SendAsync("NewPlayerJoin", successMessage.OthersInGroupMessage);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("JoinRoomError", ex.Message);
        }
    }
    public async Task LeaveRoom(string code)
    {
        try
        {

        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("LeaveRoomError", ex.Message);
        }
    }
    private Guid GetUserId()
    {
        var userIdClaim = Context.User?.FindFirst("userId")?.Value;

        if (Guid.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }

        throw new UnauthorizedAccessException();
    }
} 