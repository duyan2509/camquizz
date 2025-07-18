using CamQuizz.Application.Interfaces;
using CamQuizz.Domain.Entities;
using CamQuizz.Domain;
using Microsoft.EntityFrameworkCore;

namespace CamQuizz.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;
    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetRoleByNameAsync(UserRole name)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
    }
}