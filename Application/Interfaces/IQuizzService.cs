using CamQuizz.Application.Dtos;
using CamQuizz.Domain;

namespace CamQuizz.Application.Interfaces
{
    public interface IQuizzService
    {
        Task<QuizzDto> CreateAsync(CreateQuizzDto dtos);
        Task<QuizzDto> GetFullQuizzByIdAsync(Guid id);
        Task<QuizzInfoDto> GetQuizInfoByIdAsync(Guid id);
        Task<QuizzInfoDto> UpdateQuizInfoAsync(Guid id, UpdateQuizzDto updateQuizzDto, Guid userId);
        Task<bool> DeleteQuiz(Guid id);
        Task<PagedResultDto<QuizzInfoDto>> GetMyQuizzesAsync(int pageNumber, int pageSize, QuizzStatus? quizzStatus, Guid userId);
        // POST: /group/{groupId}/quizz/{quizzId}: create quizz share --> update access in quizz service

    }
}
