using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using CamQuizz.Infrastructure.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CamQuizz.Presentation.Hubs;
[Authorize]
public class ChatHub:Hub
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
        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
    public async Task JoinGroup(Guid groupId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());
        _groupChatManager.AddToGroup(groupId, Context.ConnectionId);
        var userId = GetUserId();
        try
        {
            var result = await _messageService.GetAllMessagesAsync(userId, groupId, 1, 20);
            await Clients.Caller.SendAsync("ReceiveOldMessage", result);        
        }
        catch (UnauthorizedAccessException ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
            _groupChatManager.RemoveFromGroup(groupId, Context.ConnectionId);
        }
    }

    public async Task LeaveGroup(Guid groupId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId.ToString());
        _groupChatManager.RemoveFromGroup(groupId, Context.ConnectionId);
    }
    public async Task LoadMoreMessages(PagedRequestDto pagedRequestDto)
    {
        var groupId = _groupChatManager.GetGroupOfConnection(Context.ConnectionId);
        if (groupId== Guid.Empty)
        {
            await Clients.Caller.SendAsync("Error", "You are not in a group chat");
            return;
        }

        try
        {
            var userId = GetUserId();
            var result =
                await _messageService.GetAllMessagesAsync(userId, groupId, pagedRequestDto.Page, pagedRequestDto.Size);
            await Clients.Caller.SendAsync("ReceiveOldMessage", result);
        }
        catch (UnauthorizedAccessException ex)
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
            if(groupId==Guid.Empty)
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
        try
        {
            var userId = GetUserId();
            var messageGroupId = _groupChatManager.GetGroupOfConnection(Context.ConnectionId);
            if(messageGroupId == Guid.Empty)
            {
                await Clients.Caller.SendAsync("Error", "You are not in a group chat");
                return;
            }
            var result = await _messageService.CreateMessageAsync(messageGroupId, userId, dto);
            await Clients.OthersInGroup(messageGroupId.ToString()).SendAsync("ReceiveNewMessage", result?.Message);
            if (result?.ReceiverIds != null)
                foreach (var receiver in result?.ReceiverIds)
                {
                    await Clients.User(receiver.ToString()).SendAsync("NotifyNewMessage", result.Message);
                }
        }
        catch (UnauthorizedAccessException ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
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