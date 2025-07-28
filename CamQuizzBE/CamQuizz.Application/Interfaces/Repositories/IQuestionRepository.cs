using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;

namespace CamQuizz.Application.Interfaces
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Task<Question> GetFullQuestionByIdAsync(Guid questionId);
        Task<PagedResultDto<Question>> GetQuestionsByQuizIdAsync(Guid quizId,string? kw ,  bool newest, int pageNumber, int pageSize);
        Task<int> GetQuestionCountByQuizIdAsync(Guid quizId);
        
    }
}
