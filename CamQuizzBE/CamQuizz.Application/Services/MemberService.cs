using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain.Entities;
using CamQuizz.Domain;
using AutoMapper;
namespace CamQuizz.Application.Services;

public class MemberService : IMemberService
{

    private readonly IMemberRepository _memberRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IQuizzShareRepository _quizzShareRepository;


    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IQuizzRepository _quizzRepository;

    public MemberService(IUnitOfWork unitOfWork
        , IMemberRepository memberRepository
        , IUserRepository userRepository
        , IGroupRepository groupRepository
        , IQuizzShareRepository quizzShareRepository
        , IMapper mapper
        ,IQuizzRepository quizzRepository)
    {
        _unitOfWork = unitOfWork;
        _memberRepository = memberRepository;
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _quizzShareRepository = quizzShareRepository;
        _mapper = mapper;
        _quizzRepository = quizzRepository;
        
    }
    // POST: /group/member: add member
    public async Task<UserGroupDto> CreateAsync(Guid ownerId, CreateMemberDto createMemberDto)
    {
        var user = await _userRepository.GetByEmailAsync(createMemberDto.Email); 
        if (user == null)
            throw new InvalidOperationException($"User is not found with email: {createMemberDto.Email} ");
        var group = await _groupRepository.GetByIdAsync(createMemberDto.GroupId);
        if (group == null)
            throw new InvalidOperationException($"Group is not found with id: {createMemberDto.GroupId}");
        if (group.OwnerId != ownerId)
            throw new InvalidOperationException("Only the group owner can add members.");
        var member = new UserGroup
        {
            UserId = user.Id,
            GroupId = createMemberDto.GroupId,
        };
        try
        {
            await _memberRepository.AddAsync(member);
            return _mapper.Map<UserGroupDto>(member);
        }
        catch (Exception ex)
        when (ex.InnerException?.Message.Contains("duplicate") == true)
        {
            throw new InvalidOperationException("User is being a member.");
        }
        catch (Exception ex)
        {
            throw new Exception("Error when add member.", ex);
        }

    }
    // DELETE: /group/{groupId}/member/{userId}: remove member
    public async Task<bool> RemoveAsync(Guid ownerId, Guid userId, Guid groupId)
    {
        var group = await _groupRepository.GetByIdAsync(groupId);
        if (group == null)
            throw new InvalidOperationException($"Group is not found with id: {groupId}");
        if (group.OwnerId != ownerId)
            throw new InvalidOperationException("Only the group owner can remove members.");
        if (group.OwnerId == userId)
            throw new InvalidOperationException("Owner can't remove owner.");
        var member = await _memberRepository.GetByUserIdGroupIdAsync(userId, groupId); 
        if (member == null)
            throw new InvalidOperationException($"User is not a group member: {groupId}");
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var quizzShares = await _quizzShareRepository.GetByUserIdGroupIdAsync(userId, groupId);
            var enumerable = quizzShares as QuizzShare[] ?? quizzShares.ToArray();
            foreach (var quizzShare in enumerable)
            {
                var quizz = await _quizzRepository.GetFullByIdAsync(quizzShare.QuizzId);
                if (quizz is { QuizzShares.Count: 1 })
                    await _quizzRepository.UpdateStatusASync(quizz, QuizzStatus.Public);
            }
            await _quizzShareRepository.DeleteRangeAsync(enumerable);
            await _memberRepository.HardDeleteAsync(member.Id);
            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new Exception("Error when removing member from group");
        }
    }
    // POST: /group/leave : leave group {groupId}
    public async Task<bool> LeaveAsync(Guid userId, Guid groupId)
    {
        var group = await _groupRepository.GetByIdAsync(groupId);
        if (group == null)
            throw new InvalidOperationException($"Group is not found with id: {groupId}");
        if(group.OwnerId==userId)
            throw new InvalidOperationException($"Owner can't leave group");

        var member = await _memberRepository.GetByUserIdGroupIdAsync(userId, groupId);
        if (member == null)
            throw new InvalidOperationException($"User is not a group member: {groupId}");
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var quizzShares = await _quizzShareRepository.GetByUserIdGroupIdAsync(userId, groupId);
            var enumerable = quizzShares as QuizzShare[] ?? quizzShares.ToArray();
            foreach (var quizzShare in enumerable)
            {
                var quizz = await _quizzRepository.GetFullByIdAsync(quizzShare.QuizzId);
                if (quizz is { QuizzShares.Count: 1 })
                    await _quizzRepository.UpdateStatusASync(quizz, QuizzStatus.Public);
            }
            await _quizzShareRepository.DeleteRangeAsync(quizzShares);
            await _memberRepository.HardDeleteAsync(member.Id);
            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new Exception("Error when leaving member from group");
        }
    }

    // GET: /group/{groupId}/members: group with group user
    public async Task<Dtos.PagedResultDto<MemberDto>> GetGroupMembersAsync(int page, int size, Guid groupId, Guid userId)
    {

        var group = await _groupRepository.GetByIdAsync(groupId);
        if (group == null)
            throw new InvalidOperationException($"Group is not found with id: {groupId}");
        var member = await _memberRepository.GetByUserIdGroupIdAsync(userId, groupId);
        if (member == null)
            throw new InvalidOperationException($"User is not a group member: {groupId}");
        var result = await _memberRepository.GetByGroupIdAsync(page, size, groupId); 
        var members = result.Data
            .Select(ug => _mapper.Map<MemberDto>(ug.User))
            .ToList();
        members.ForEach(m => m.IsOwner = m.Id == group.OwnerId);
        return new PagedResultDto<MemberDto>
            {
                Data = members,
                Page = page,
                Size = size,
                Total = result.Total
            };

    }

    public async Task<PagedResultDto<ConversationPreview>> GetConversationsAsync(int page, int size, Guid userId)
    {
        return await _memberRepository.GetAllConversationsAsync(page,size,userId);
    }
}