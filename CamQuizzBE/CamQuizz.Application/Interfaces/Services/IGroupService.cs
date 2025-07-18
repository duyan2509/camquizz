using CamQuizz.Application.Dtos;

namespace CamQuizz.Application.Interfaces;

public interface IGroupService
{
    Task<GroupDto> UpdateAsync(Guid ownerId, Guid id, UpdateGroupDto updateGroupDto);
    Task<FullGroupDto> CreateAsync(Guid userId, CreateGroupDto createGroupDto);
    Task<FullGroupDto> GetFullByIdAsync(Guid guid);
    Task<GroupDto> GetByIdAsync(Guid userId, Guid guid);
    
    Task<bool> DeleteAsync(Guid userId, Guid guid);
    Task<PagedResultDto<GroupDto>> GetGroupsAsync(string query, int page, int size, bool? isOwner, Guid userId);
    Task<PagedResultDto<GroupQuizzInfoDto>> GetGroupQuizzesAsync(int page, int size, string? kw, Guid userId, Guid groupId);

    Task<bool> UpdateVisibleAsync(Guid userId, Guid groupId, Guid quizId, bool visible);
    Task<List<GroupDto>> GetQuizzGroupsAsync(Guid userId, Guid quizId, bool shared);
    Task<bool> DeleteQuizzShareAsync(Guid userId, Guid groupId, Guid quizId);
}