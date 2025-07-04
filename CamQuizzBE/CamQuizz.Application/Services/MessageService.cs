using AutoMapper;
using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CamQuizz.Application.Services;

public class MessageService:IMessageService
{
    private readonly IMapper _mapper;
    private readonly IMemberRepository _memberRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MessageService(IMapper mapper, IMemberRepository memberRepository, IMessageRepository messageRepository, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _memberRepository = memberRepository;
        _messageRepository = messageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResultDto<MessageDto>> GetAllMessagesAsync(Guid userId, Guid groupId, int page, int size)
    {
        var member = await _memberRepository.GetByUserIdGroupIdAsync(userId, groupId);
        if (member == null)
            throw new UnauthorizedAccessException("Only member can view group message");
        var result = await _messageRepository.GetGroupMessageAsync(groupId, page, size);
        var messages = result.Data.Select(x => _mapper.Map<MessageDto>(x)).ToList();
        return new PagedResultDto<MessageDto>
        {
            Data = messages,
            Page = page,
            Size = size,
            Total = result.Total
        };
    }

    public async Task<CreateMessageResultDto?> CreateMessageAsync(Guid groupId, Guid senderId, CreateMessageDto dto)
    {
        var member = await _memberRepository.GetByUserIdGroupIdAsync(senderId, groupId);
        if (member == null)
            throw new UnauthorizedAccessException("Only member can send message");
        var message = new GroupMessage
        {
            UserId = senderId,
            GroupId = groupId,
            Message = dto.Content,
        };
        await _messageRepository.AddAsync(message);
        var messageDto = await GetByIdAsync(message.Id);
        var result = await _memberRepository.GetAllReceiversAsync(groupId,senderId);
        List<Guid> receiverIds = result.Select(x => x.Id).ToList();
        return new CreateMessageResultDto
        {
            Message = messageDto,
            ReceiverIds = receiverIds
        };
    }

    public async Task MarkAsReadAsync(Guid messageId, Guid userId, Guid groupId)
    {
        var message = await _messageRepository.GetByIdAsync(messageId);
        if(message == null)
            throw new NotImplementedException("Message is not found");
        var member = await _memberRepository.GetByUserIdGroupIdAsync(userId, groupId);
        if (member == null)
            throw new UnauthorizedAccessException("User is not in group");
        await _memberRepository.UpdateLastReadMessage(member, messageId);
    }

    public async Task<MessageDto?> GetByIdAsync(Guid messageId)
    {
        var message = await _messageRepository.GetUserMessageAsync(messageId);
        return _mapper.Map<MessageDto>(message);
    }
}