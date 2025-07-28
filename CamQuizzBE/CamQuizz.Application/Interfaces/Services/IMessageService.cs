using CamQuizz.Application.Dtos;

namespace CamQuizz.Application.Interfaces;

public interface IMessageService
{ 
    Task<PagedResultMessgeDto<MessageDto>> GetAllMessagesAsync(DateTime? afterCreatedAt, Guid? afterId, Guid userId, Guid groupId, int size);
    Task<CreateMessageResultDto?> CreateMessageAsync(Guid groupId, Guid senderId, CreateMessageDto dto);
    Task MarkAsReadAsync(Guid messageId, Guid userId, Guid groupId);
    Task<MessageDto?> GetByIdAsync(Guid messageId);
}