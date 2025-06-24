using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;

namespace CamQuizz.Application.Interfaces;

public interface IMemberService
{
    // POST: /group/member: add member
    Task<UserGroupDto> CreateAsync(Guid ownerId, CreateMemberDto createMemberDto);
    // DELETE: /group/{groupId}/member/{userId}: remove member
    Task<bool> RemoveAsync(Guid ownerId, Guid userId, Guid groupId);
    // POST: /group/leave : leave group {groupId}
    Task<bool> LeaveAsync(Guid userId, Guid groupId);

    // GET: /group/{groupId}/members: group with group user
    Task<PagedResultDto<MemberDto>> GetGroupMembersAsync(int page, int size, Guid groupId, Guid userId);    
}