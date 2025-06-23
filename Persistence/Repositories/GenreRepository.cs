using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CamQuizz.Persistence.Repositories;

public class GenreRepository : GenericRepository<Genre>, IGenreRepository
{
    public GenreRepository(ApplicationDbContext context, ILogger<Genre> logger)
        : base(context, logger)
    {
    }

    public async Task<bool> CheckExistNameAsync(Guid id, string name) {
        return await _dbSet.AnyAsync(g => g.Name == name && g.Id != id);
    }
    public async Task<bool> CheckExistNameAsync(string name) {
        return await _dbSet.AnyAsync(g => g.Name == name);
    }
}
