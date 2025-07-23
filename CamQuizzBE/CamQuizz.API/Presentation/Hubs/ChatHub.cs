using CamQuizz.Application.Dtos;
using CamQuizz.Application.Exceptions;
using CamQuizz.Application.Interfaces;
using CamQuizz.Infrastructure.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CamQuizz.Presentation.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly GroupChatConnectionManager _groupChatManager;
    private readonly IMessageService _messageService;

    public ChatHub(GroupChatConnectionManager groupChatManager, IMessageService messageService)
    {
        _groupChatManager = groupChatManager;
        _messageService = messageService;
    }
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        Console.WriteLine($"User connected: {userId}, ConnectionId: {Context.ConnectionId}");

        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
    public async Task JoinGroup(JoinGroupDto dto)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, dto.GroupId.ToString());
        _groupChatManager.AddToGroup(dto.GroupId, Context.ConnectionId);
        var userId = GetUserId();
        try
        {
            var result = await _messageService.GetAllMessagesAsync(dto.AfterCreatedAt,dto.AfterId,userId, dto.GroupId, 20);
            await Clients.Caller.SendAsync("ReceiveFirstMessages", result);
        }
        catch (ForbiddenException ex)
        {
            await Clients.Caller.SendAsync("JoinGroupError", ex.Message);
            _groupChatManager.RemoveFromGroup(dto.GroupId, Context.ConnectionId);
        }
    }

    public async Task LeaveGroup(Guid groupId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId.ToString());
        _groupChatManager.RemoveFromGroup(groupId, Context.ConnectionId);
    }
    public async Task LoadMoreMessages(PagedRequestKeysetDto dto)
    {
        var groupId = _groupChatManager.GetGroupOfConnection(Context.ConnectionId);
        if (groupId == Guid.Empty)
        {
            await Clients.Caller.SendAsync("Error", "You are not in a group chat");
            return;
        }

        try
        {
            var userId = GetUserId();
            var result =
                await _messageService.GetAllMessagesAsync(dto.AfterCreatedAt,dto.AfterId,userId, groupId, dto.Size);
            Console.WriteLine($"{result.Total} - {result.GroupName}");
            await Clients.Caller.SendAsync("ReceiveOldMessage", result);
        }
        catch (ForbiddenException ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
            _groupChatManager.RemoveFromGroup(groupId, Context.ConnectionId);
        }
    }
    public async Task MarkLastRead(Guid messageId)
    {
        try
        {
            if (messageId == Guid.Empty)
                return;
            var userId = GetUserId();
            var groupId = _groupChatManager.GetGroupOfConnection(Context.ConnectionId);
            if (groupId == Guid.Empty)
            {
                await Clients.Caller.SendAsync("Error", "You are not in a group chat");
                return;
            }
            await _messageService.MarkAsReadAsync(messageId, userId, groupId);
        }
        catch (UnauthorizedAccessException ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
    }

    public async Task SendMessage(CreateMessageDto dto)
    {
        var userId = GetUserId();
        if (userId == Guid.Empty)
        {
            await Clients.Caller.SendAsync("ErrorSendMessage", "Invalid user");
            return;
        }

        if (string.IsNullOrWhiteSpace(dto.Content))
        {
            await Clients.Caller.SendAsync("ErrorSendMessage", "Invalid message content");
            return;
        }
        try
        {
            var messageGroupId = _groupChatManager.GetGroupOfConnection(Context.ConnectionId);
            if (messageGroupId == Guid.Empty)
            {
                await Clients.Caller.SendAsync("ErrorSendMessage", "You are not a group member [Connection]");
                return;
            }
            var result = await _messageService.CreateMessageAsync(messageGroupId, userId, dto);
            await Clients.Caller.SendAsync("SendSuccess", result?.Message);
            await Clients.OthersInGroup(messageGroupId.ToString()).SendAsync("ReceiveNewMessage", result?.Message);
            await Clients.Groups(messageGroupId.ToString()).SendAsync("UpdatePreview",true);
            if (result?.ReceiverIds != null)
            {
                var receiverUserIds = result.ReceiverIds.Select(id => id.ToString());
                await Clients.Users(receiverUserIds).SendAsync("NotifyNewMessage", new { result.Message, result.GroupName });

            }

        }
        catch (UnauthorizedAccessException ex)
        {
            await Clients.Caller.SendAsync("ErrorSendMessage", ex.Message);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("ErrorSendMessage", "Unexpected error occurred");
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
    public string GetConnectionId() => Context.ConnectionId;
}