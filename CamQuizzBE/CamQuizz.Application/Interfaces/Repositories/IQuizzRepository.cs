using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;
using CamQuizz.Domain;

namespace CamQuizz.Application.Interfaces
{
    public interface IQuizzRepository : IGenericRepository<Quizz>
    {
        Task<Quizz?> GetInfoByIdAsync(Guid id);
        Task<Quizz?> GetDetailByIdAsync(Guid id);
        
        Task<Quizz?> GetFullByIdAsync(Guid id);
        Task<Quizz?> GetAccessQuizzByIdAsync(Guid id);
        
        Task<PagedResultDto<Quizz>> GetMyQuizzesAsync(string? kw, Guid? categoryId, bool popular, bool newest,int pageNumber, int pageSize, QuizzStatus? quizzStatus, Guid userId);
        Task<PagedResultDto<Quizz>> GetAllQuizzesAsync(string? kw, Guid? categoryId, bool popular, bool newest,int pageNumber, int pageSize);

        Task<Quizz> UpdateStatusASync(Quizz quizz, QuizzStatus newStatus);
    }
}
