using CamQuizz.Application.Dtos;
using CamQuizz.Domain;

namespace CamQuizz.Application.Interfaces
{
    public interface IQuizzService
    {
        Task<QuizzDto> CreateAsync(Guid authorId, CreateQuizzDto dtos);
        Task<QuizzDto> GetFullQuizzByIdAsync(Guid id);
        Task<QuizzInfoDto> GetQuizInfoByIdAsync(Guid id);
        Task<DetailQuizDto> GetDetailByIdAsync(Guid id);
        
        Task<QuizzInfoDto> UpdateQuizInfoAsync(Guid id, UpdateQuizzDto updateQuizzDto, Guid userId);
        Task<bool> DeleteQuiz(Guid userId, Guid id);
        Task<PagedResultDto<QuizzInfoDto>> GetMyQuizzesAsync(string? kw, Guid? categoryId, bool popular, bool newest, int pageNumber, int pageSize, QuizzStatus? quizzStatus, Guid userId);
        Task<PagedResultDto<QuizzInfoDto>> GetAllQuizzesAsync(string? kw, Guid? categoryId, bool popular, bool newest,  int pageNumber, int pageSize);
        
        Task<QuizzAccessDto> GetQuizzAccessAsync(Guid quizzId, Guid userId);
        Task<QuizzAccessDto> UpdateQuizzAccessAsync(Guid quizzId, Guid userId, UpdateAccessDto updateAccessDto);
        
    }
}
