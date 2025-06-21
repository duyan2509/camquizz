using CamQuizz.Application.Dtos;
using CamQuizz.Domain;
using CamQuizz.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CamQuizz.Persistence.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
    Task<IEnumerable<User>> GetActiveUsersAsync();
    Task<PagedResultDto<User>> GetPagedAsync(int pageNumber, int pageSize);
}
