using CamQuizz.Application.Dtos;
using CamQuizz.Domain;
using CamQuizz.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CamQuizz.Persistence.Interfaces;

public interface IRoleRepository 
{
    Task<Role?> GetRoleByNameAsync(UserRole name);
}
