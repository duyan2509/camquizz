using CamQuizz.Domain;
using CamQuizz.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CamQuizz.Persistence.Interfaces;

public interface IGenreRepository : IGenericRepository<Genre>
{
    Task<bool> CheckExistNameAsync(Guid id, string name);
    Task<bool> CheckExistNameAsync(string name);
}
