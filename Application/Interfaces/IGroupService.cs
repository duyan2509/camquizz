using CamQuizz.Application.Dtos;

namespace CamQuizz.Application.Interfaces;

public interface IGroupService
{
    Task<GroupDto> UpdateAsync(Guid ownerId, Guid id, UpdateGroupDto updateGroupDto);
    Task<FullGroupDto> CreateAsync(Guid userId, CreateGroupDto createGroupDto);
    Task<FullGroupDto> GetByIdAsync(Guid guid);
    Task<bool> DeleteAsync(Guid userId, Guid guid);
    Task<PagedResultDto<GroupDto>> GetGroupsAsync(int page, int size, bool? isOwner, Guid userId);
    Task<PagedResultDto<QuizzInfoDto>> GetGroupQuizzesAsync(int page, int size, Guid userId, Guid groupId);
    // group service 
    // POST: /group
    // PUT: /group
    // DELETE: /group
    // GET:  /group || /group?isOwner=true || /group?isOwner=false  
    // GET: /group/{groupId}/quizz : group with quizz share
    // PUT: /group/{groupId}/quizz/{quizzId}: show / hide quizz


}