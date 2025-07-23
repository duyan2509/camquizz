using AutoMapper;
using CamQuizz.Application.Dtos;
using CamQuizz.Application.Exceptions;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain.Entities;


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

    public async Task<PagedResultMessgeDto<MessageDto>> GetAllMessagesAsync(DateTime? afterCreatedAt, Guid? afterId, Guid userId, Guid groupId, int size)
    {
        var member = await _memberRepository.GetByUserIdGroupIdAsync(userId, groupId);
        if (member == null)
            throw new ForbiddenException("Only member can view group message");
        
        var result = await _messageRepository.GetGroupMessageAsync(groupId, afterCreatedAt, afterId, size);
        var messages = result.Data.Select(x => _mapper.Map<MessageDto>(x)).ToList();
        foreach (var messageDto in messages)
        {
            Console.WriteLine($"create at: {messageDto.Time}");
        }
        return new PagedResultMessgeDto<MessageDto>
        {
            Data = messages,
            Page = 0,
            Size = size,
            Total = result.Total,
            GroupName = member.Group.Name,
            GroupId = member.GroupId
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
        await _memberRepository.UpdateLastReadMessage(member, message.Id);
        var messageDto = await GetByIdAsync(message.Id);
        var result = await _memberRepository.GetAllReceiversAsync(groupId,senderId);
        List<Guid> receiverIds = result.Select(x => x.UserId).ToList();
        return new CreateMessageResultDto
        {
            GroupName = member.Group.Name,
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