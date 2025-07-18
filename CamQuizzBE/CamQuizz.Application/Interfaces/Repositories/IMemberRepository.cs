using CamQuizz.Domain.Entities;
using CamQuizz.Application.Dtos;

namespace CamQuizz.Application.Interfaces
{
    public interface IMemberRepository : IGenericRepository<UserGroup>
    {
        Task<UserGroup?> GetByUserIdGroupIdAsync(Guid userId, Guid groupId);
        Task<PagedResultDto<UserGroup>> GetByGroupIdAsync(int page, int size, Guid groupId);
        Task UpdateLastReadMessage(UserGroup member, Guid messageId);
        Task<List<UserGroup>> GetAllReceiversAsync(Guid groupId, Guid senderId);
        Task<IEnumerable<UserGroup>> GetAllMembersAsync(Guid groupId);
        Task<PagedResultDto<ConversationPreview>> GetAllConversationsAsync(int page, int size, Guid userId);

    }
}
