using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CamQuizz.Presentation.Controllers;

public class AuthController : BaseController
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            var user = await _userService.CreateAsync(createUserDto);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var response = await _userService.LoginAsync(loginDto);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userId = GetCurrentUserId();
        var user = await _userService.GetByIdAsync(userId);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpGet("users/{id}")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPut("users/{id}")]
    [Authorize]
    public async Task<ActionResult<UserDto>> UpdateUser(
        Guid id,
        [FromBody] UpdateUserDto updateUserDto
    )
    {
        // Only allow users to update their own profile
        if (GetCurrentUserId() != id)
            return Forbid();

        try
        {
            var user = await _userService.UpdateAsync(id, updateUserDto);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("check-username/{username}")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> CheckUsername(string username)
    {
        var exists = await _userService.UsernameExistsAsync(username);
        return Ok(exists);
    }

    [HttpGet("check-email/{email}")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> CheckEmail(string email)
    {
        var exists = await _userService.EmailExistsAsync(email);
        return Ok(exists);
    }

    [HttpGet("users")]
    [Authorize]
    public async Task<ActionResult<PagedResultDto<UserDto>>> GetUsers(
        [FromQuery] PagedRequestDto request
    )
    {
        var result = await _userService.GetPagedAsync(request.Page, request.Size);
        return Ok(result);
    }
}
