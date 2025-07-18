using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;

namespace CamQuizz.Application.Interfaces
{
    public interface IGroupRepository : IGenericRepository<Group>
    {
        Task<Group?> GetFullGroupByIdAsync(Guid id);
        Task<Group?> GetGroupInfoIdAsync(Guid id);
        Task<PagedResultDto<Group>> GetMemberGroupsAsync(string kw, int page, int size, Guid userId);
        Task<PagedResultDto<Group>> GetOwnerGroupsAsync(string kw, int page, int size, Guid userId);
        Task<PagedResultDto<Group>> GetAllGroupsAsync(string kw, int page, int size, Guid userId);
        Task<List<Group>> GetAllGroupsAsync(Guid userId);
        Task<List<Group>> GetQuizzGroupsAsync(Guid quizzId, bool hide);
        Task<Group> UpdateNameAsync(Group group);
        Task<Group> CreateAsync(Group group);


    }
}
