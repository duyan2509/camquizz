using CamQuizz.Application.Dtos;

namespace CamQuizz.Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> CreateAsync(CreateUserDto createUserDto);
    Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto);
    Task DeleteAsync(Guid id);
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
    Task<PagedResultDto<UserDto>> GetPagedAsync(int pageNumber, int pageSize);
} 