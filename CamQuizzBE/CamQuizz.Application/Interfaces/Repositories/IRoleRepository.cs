using CamQuizz.Domain;
using CamQuizz.Domain.Entities;

namespace CamQuizz.Application.Interfaces;

public interface IRoleRepository 
{
    Task<Role?> GetRoleByNameAsync(UserRole name);
}
