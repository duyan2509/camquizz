using System.Security.Cryptography;
using System.Text;
using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain.Entities;
using CamQuizz.Domain;

namespace CamQuizz.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;
    private readonly IRoleRepository _roleRepository;

    public UserService(IUserRepository userRepository, JwtService jwtService, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _roleRepository = roleRepository;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user != null ? MapToDto(user) : null;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
    {
        if (await _userRepository.UsernameExistsAsync(createUserDto.Username))
            throw new InvalidOperationException("Username already exists");

        if (await _userRepository.EmailExistsAsync(createUserDto.Email))
            throw new InvalidOperationException("Email already exists");
        var role = await _roleRepository.GetRoleByNameAsync(UserRole.User);
        var user = new User
        {
            Username = createUserDto.Username,
            Email = createUserDto.Email,
            PasswordHash = HashPassword(createUserDto.Password),
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            PhoneNumber = createUserDto.PhoneNumber,
            RoleId = role.Id
        };

        var createdUser = await _userRepository.AddAsync(user);
        return MapToDto(createdUser);
    }

    public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new InvalidOperationException("User not found");

        if (updateUserDto.FirstName != null)
            user.FirstName = updateUserDto.FirstName;
        if (updateUserDto.LastName != null)
            user.LastName = updateUserDto.LastName;
        if (updateUserDto.PhoneNumber != null)
            user.PhoneNumber = updateUserDto.PhoneNumber;

        var updatedUser = await _userRepository.UpdateAsync(user);
        return MapToDto(updatedUser);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _userRepository.SoftDeleteAsync(id);
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
        if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
            throw new InvalidOperationException("Invalid username or password");

        if (!user.IsActive)
            throw new InvalidOperationException("User account is deactivated");

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        var token = _jwtService.GenerateToken(user.Id, user.Username, user.Email, user.Role.Name);
        var expiresAt = DateTime.UtcNow.AddMinutes(60); // Default 60 minutes

        return new LoginResponseDto
        {
            Token = token,
            User = MapToDto(user),
            ExpiresAt = expiresAt,
        };
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _userRepository.UsernameExistsAsync(username);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _userRepository.EmailExistsAsync(email);
    }

    public async Task<PagedResultDto<UserDto>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var pagedResult = await _userRepository.GetPagedAsync(pageNumber, pageSize);
        return new PagedResultDto<UserDto>
        {
            Data = pagedResult.Data.Select(MapToDto),
            Total = pagedResult.Total,
            Page = pagedResult.Page,
            Size = pagedResult.Size,
        };
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt,
            Role = user.Role?.Name.ToString() ?? UserRole.User.ToString()
        };
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }
}
