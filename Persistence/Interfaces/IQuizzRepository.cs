using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Repositories;

namespace CamQuizz.Persistence.Interfaces
{
    public interface IQuizzRepository: IGenericRepository<Quizz>
    {
        Task<Quizz?> GetInfoByIdAsync(Guid id);
        Task<Quizz?> GetFullByIdAsync(Guid id);

    }
}
