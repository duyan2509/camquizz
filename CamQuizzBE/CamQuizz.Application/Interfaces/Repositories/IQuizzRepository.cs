using CamQuizz.Domain.Entities;
using CamQuizz.Domain;

namespace CamQuizz.Persistence.Interfaces
{
    public interface IQuizzRepository : IGenericRepository<Quizz>
    {
        Task<Quizz?> GetInfoByIdAsync(Guid id);
        Task<Quizz?> GetFullByIdAsync(Guid id);
        Task<Quizz?> GetAccessQuizzByIdAsync(Guid id);
        
        Task<PagedResultDto<Quizz>> GetMyQuizzesAsync(int pageNumber, int pageSize, QuizzStatus? quizzStatus, Guid userId);
        Task<Quizz> UpdateStatusASync(Quizz quizz, QuizzStatus newStatus);
    }
}
