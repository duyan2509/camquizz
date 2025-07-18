using CamQuizz.Domain.Entities;

namespace CamQuizz.Application.Interfaces
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Task<Question> GetFullQuestionByIdAsync(Guid questionId);
    }
}
