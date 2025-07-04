using CamQuizz.Application.Dtos;
using CamQuizz.Domain;
namespace CamQuizz.Application.Interfaces
{
    public interface IQuestionService
    {
        Task<QuestionDto> CreateAsync(CreateQuestionDto createQuestionDto, Guid quizzId, Guid userId);
        Task<QuestionDto> GetQuestionById(Guid questionId);
        Task<bool> DeleteAsync(Guid quizzId,Guid questionId, Guid userId);
        Task<QuestionDto> UpdateAsync(Guid quizzId,QuestionDto questionDto, Guid userId);
        Task<PagedResultDto<QuestionDto>> GetAllQuestionsAsync(Guid quizzId, Guid userId, int page, int size);
    }
}
