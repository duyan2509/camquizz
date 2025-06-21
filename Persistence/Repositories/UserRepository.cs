using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CamQuizz.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context)
        : base(context) { }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _dbSet.AnyAsync(u => u.Username == username);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbSet.AnyAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        return await _dbSet.Where(u => u.IsActive).ToListAsync();
    }


}
