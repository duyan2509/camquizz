using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CamQuizz.Presentation.Controllers;

[Authorize]

public class GroupController : BaseController
{
    private readonly IGroupService _groupService;
    private readonly IMemberService _memberService;
    private readonly IQuizzService _quizzService;


    public GroupController(IGroupService groupService, IMemberService memberService, IQuizzService quizzService)
    {
        _groupService = groupService;
        _memberService = memberService;
        _quizzService = quizzService;
    }

    [HttpPost]
    public async Task<ActionResult<FullGroupDto>> CreateGroup([FromBody] CreateGroupDto createGroupDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            Guid userId = GetCurrentUserId();
            var response = await _groupService.CreateAsync(userId, createGroupDto);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<GroupDto>> UpdateGroup(Guid id, [FromBody] UpdateGroupDto updateGroupDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            Guid userId = GetCurrentUserId();
            var response = await _groupService.UpdateAsync(userId, id, updateGroupDto);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<FullGroupDto>> GetById(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var response = await _groupService.GetByIdAsync(id);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteById(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            Guid userId = GetCurrentUserId();
            var result = await _groupService.DeleteAsync(userId, id);
            if (result)
                return Ok(new { success = true });
            else
                return StatusCode(500, new { message = "Unexpected error during group removal." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("my-groups")]
    public async Task<ActionResult<PagedResultDto<GroupDto>>> GetGroups(
               [FromQuery] PagedRequestDto request, [FromQuery] bool? isOwner
        )
    {
        Guid userId = GetCurrentUserId();
        var result = await _groupService.GetGroupsAsync(request.Page, request.Size, isOwner, userId);
        return Ok(result);
    }
    [HttpGet("{groupId}/quizz")]
    public async Task<ActionResult<PagedResultDto<GroupQuizzInfoDto>>> GetGroupQuizzes(
               [FromQuery] PagedRequestDto request, [FromRoute] Guid groupId
        )
    {
        try
        {
            Guid userId = GetCurrentUserId();
            var result = await _groupService.GetGroupQuizzesAsync(request.Page, request.Size, userId, groupId);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpPost("member")]
    public async Task<ActionResult<UserGroupDto>> AddMember([FromBody] CreateMemberDto createMemberDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            Guid userId = GetCurrentUserId();
            var response = await _memberService.CreateAsync(userId, createMemberDto);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpDelete("{groupId}/member/{userId}")]
    public async Task<IActionResult> RemoveMembers([FromRoute] Guid userId, [FromRoute] Guid groupId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            Guid ownerId = GetCurrentUserId();
            var result = await _memberService.RemoveAsync(ownerId, userId, groupId);
            if (result)
                return Ok(new { success = true });
            else
                return StatusCode(500, new { message = "Unexpected error during member removal." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
        }

    }
    [HttpPost("leave")]
    public async Task<IActionResult> LeaveGroup([FromBody] LeaveGroupDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            Guid userId = GetCurrentUserId();
            var result = await _memberService.LeaveAsync(userId, dto.GroupId);
            if (result)
                return Ok(new { success = true });
            else
                return StatusCode(500, new { message = "Unexpected error during member removal." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
        }

    }
    [HttpGet("{groupId}/members")]
    public async Task<ActionResult<PagedResultDto<MemberDto>>> GetGroupMembers(
               [FromQuery] PagedRequestDto request, [FromRoute] Guid groupId
        )
    {
        try
        {
            Guid userId = GetCurrentUserId();
            var result = await _memberService.GetGroupMembersAsync(request.Page, request.Size, groupId, userId);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{groupId}/quizz/{quizzId}")]
    public async Task<ActionResult<GroupDto>> UpdateQuizzVisible(Guid groupId, Guid quizzId, [FromBody] UpdateQuizzVisibleDto updateQuizzVisibleDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            Guid userId = GetCurrentUserId();
            var response = await _groupService.UpdateVisibleAsync(userId, groupId, quizzId, updateQuizzVisibleDto.Visible);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpDelete("{groupId}/quizz/{quizzId}")]
    public async Task<IActionResult> DeleteQuizzFromGroup(Guid groupId, Guid quizzId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            Guid ownerId = GetCurrentUserId();
            var result = await _groupService.DeleteQuizzShareAsync(ownerId, groupId, quizzId);
            if (result)
                return Ok(new { success = true });
            else
                return StatusCode(500, new { message = "Unexpected error during member removal." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
        }

    }


}
