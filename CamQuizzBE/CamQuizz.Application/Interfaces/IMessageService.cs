using CamQuizz.Application.Dtos;

namespace CamQuizz.Application.Interfaces;

public interface IMessageService
{ 
    Task<PagedResultDto<MessageDto>> GetAllMessagesAsync(Guid userId, Guid groupId,int page, int size);
    Task<MessageDto?> CreateMessageAsync(Guid groupId, Guid senderId, CreateMessageDto dto);
    Task MarkAsReadAsync(Guid messageId, Guid userId, Guid groupId);
    Task<MessageDto?> GetByIdAsync(Guid messageId);
}