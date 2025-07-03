using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Repositories;

namespace CamQuizz.Persistence.Interfaces
{
    public interface IMemberRepository : IGenericRepository<UserGroup>
    {
        Task<UserGroup> GetByUserIdGroupIdAsync(Guid userId, Guid groupId);
        Task<PagedResultDto<UserGroup>> GetByGroupIdAsync(int page, int size, Guid groupId);
    }
}
