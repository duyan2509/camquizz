using CamQuizz.Domain;
using CamQuizz.Domain.Entities;

namespace CamQuizz.Application.Interfaces;

public interface IGenreRepository : IGenericRepository<Genre>
{
    Task<bool> CheckExistNameAsync(Guid id, string name);
    Task<bool> CheckExistNameAsync(string name);
}
