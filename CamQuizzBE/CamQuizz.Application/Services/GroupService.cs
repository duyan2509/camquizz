using System.Text;
using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain.Entities;
using CamQuizz.Domain;
using CamQuizz.Persistence.Interfaces;
using CamQuizz.Persistence;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace CamQuizz.Application.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IQuizzShareRepository _quizzShareRepository;
    private readonly IQuizzRepository _quizzRepository;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public GroupService(ApplicationDbContext context
        , IGroupRepository groupRepository
        , IMemberRepository memberRepository
        , IQuizzShareRepository quizzShareRepository
        , IMapper mapper
        ,IQuizzRepository quizzRepository
        ,IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _memberRepository = memberRepository;
        _quizzShareRepository = quizzShareRepository;
        _quizzRepository = quizzRepository;
        _mapper = mapper;
        _context = context;
        _unitOfWork = unitOfWork;
    }
    public async Task<GroupDto> UpdateAsync(Guid userId, Guid id, UpdateGroupDto updateGroupDto)
    {
        var group = await _groupRepository.GetGroupInfoIdAsync(id);
        if (group == null)
            throw new InvalidOperationException("Group doesn't exist");
        if (group.OwnerId != userId)
            throw new InvalidOperationException("Only Owner can update Group");
        group.Name = updateGroupDto.Name;
        try
        {
            await _groupRepository.UpdateAsync(group);
            return _mapper.Map<GroupDto>(group);
        }
        catch (DbUpdateException ex)
        when (ex.InnerException is SqlException sqlEx &&
                                            (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            throw new InvalidOperationException("Group name is existed for this user.");
        }

    }
    public async Task<FullGroupDto> CreateAsync(Guid ownerId, CreateGroupDto createGroupDto)
    {
        var group = new Group
        {
            OwnerId = ownerId,
            Name = createGroupDto.Name
        };
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _groupRepository.AddAsync(group);
            var member = new UserGroup
            {
                UserId = ownerId,
                GroupId = group.Id
            };
            await _memberRepository.AddAsync(member);
            transaction.CommitAsync();
            return await GetByIdAsync(group.Id);
        }
        catch (DbUpdateException ex)
        when (ex.InnerException is SqlException sqlEx &&
                                            (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            transaction.RollbackAsync();
            throw new InvalidOperationException("Group name is existed for this user.");
        }
    }
    public async Task<FullGroupDto> GetByIdAsync(Guid guid)
    {
        var group = await _groupRepository.GetFullGroupByIdAsync(guid);

        if (group == null)
            throw new InvalidOperationException("Group is not found");
        return _mapper.Map<FullGroupDto>(group);
    }
    public async Task<bool> DeleteAsync(Guid userId, Guid guid)
    {
        var group = await _groupRepository.GetByIdAsync(guid);
        if (group == null)
            throw new InvalidOperationException("Group is not found");
        if (group.OwnerId != userId)
            throw new InvalidOperationException("Only Owner can delete Group");
        await _groupRepository.HardDeleteAsync(guid);

        return true;

    }
    public async Task<Dtos.PagedResultDto<GroupDto>> GetGroupsAsync(int page, int size, bool? isOwner, Guid userId)
    {
        var result = isOwner switch
        {
            true => await _groupRepository.GetOwnerGroupsAsync(page, size, userId),
            false => await _groupRepository.GetMemberGroupsAsync(page, size, userId),
            _ => await _groupRepository.GetAllGroupsAsync(page, size, userId),
        };
        var groups = result.Data
            .Select(group => _mapper.Map<GroupDto>(group))
            .ToList();
        return new Dtos.PagedResultDto<GroupDto>
        {
            Data = groups,
            Page = result.Page,
            Total = result.Total,
            Size = result.Size
        };
    }
    public async Task<Dtos.PagedResultDto<GroupQuizzInfoDto>> GetGroupQuizzesAsync(int page, int size, Guid userId, Guid groupId)
    {
        var member = await _memberRepository.GetByUserIdGroupIdAsync(userId, groupId);
        if (member == null)
            throw new InvalidOperationException("Only Member can view shared quizzes");
        var result = await _quizzShareRepository.GetQuizzesByGroupIdAsync(page, size, groupId, userId);
        var quizzes = result.Data
            .Select(quizz => _mapper.Map<GroupQuizzInfoDto>(quizz))
            .ToList();
        return new Dtos.PagedResultDto<GroupQuizzInfoDto>
        {
            Data = quizzes,
            Page = result.Page,
            Total = result.Total,
            Size = result.Size
        };
    }
    public async Task<bool> UpdateVisibleAsync(Guid userId, Guid groupId, Guid quizId, bool visible)
    {
        var quizzShare = await _quizzShareRepository.GetByQuizzIdGroupIdAsync(quizId,groupId);
        if (quizzShare == null)
            throw new InvalidOperationException("Quiz share is not found");
        if (userId != quizzShare.Group.OwnerId) 
            throw new UnauthorizedAccessException("Only Owner can update visible Group Quizz");
        await _quizzShareRepository.UpdateVisibleAsync(quizzShare, visible);
        return true;
    }

    public async Task<List<GroupDto>> GetQuizzGroupsAsync(Guid userId, Guid quizId,  bool shared)
    {
        var groups=  await _groupRepository.GetQuizzGroupsAsync(quizId, shared);
        return _mapper.Map<List<GroupDto>>(groups);
    }

    public async Task<bool> DeleteQuizzShareAsync(Guid userId, Guid groupId, Guid quizId)
    {
        var quizzShare = await _quizzShareRepository.GetByQuizzIdGroupIdAsync(quizId,groupId);
        if (quizzShare == null)
            throw new InvalidOperationException("Quiz share is not found");
        if(quizzShare.UserId != userId)
            throw new InvalidOperationException("Only Author can remove quizz from group");
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _quizzShareRepository.HardDeleteAsync(quizzShare.Id);
            var quizzShares = await _quizzShareRepository.GetByQuizzIdAsync(quizId);
            if (!quizzShares.Any())
            {
                var quizz = await _quizzRepository.GetByIdAsync(quizId);
                if (quizz != null)
                    await _quizzRepository.UpdateStatusASync(quizz, QuizzStatus.Public);
            }

            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new InvalidOperationException($"Error when delete quizz from group: {ex.Message}");
        }
    }
}