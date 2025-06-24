using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Repositories;

namespace CamQuizz.Persistence.Interfaces
{
    public interface IGroupRepository : IGenericRepository<Group>
    {
        Task<Group> GetFullGroupByIdAsync(Guid id);
        Task<Group> GetGroupInfoIdAsync(Guid id);
        Task<PagedResultDto<Group>> GetMemberGroupsAsync(int page, int size, Guid userId);
        Task<PagedResultDto<Group>> GetOwnerGroupsAsync(int page, int size, Guid userId);
        Task<PagedResultDto<Group>> GetAllGroupsAsync(int page, int size, Guid userId);

    }
}
