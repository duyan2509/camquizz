using CamQuizz.Application.Dtos;

namespace CamQuizz.Application.Interfaces
{
    public interface IQuizzService
    {
        Task<QuizzDto> CreateAsync(CreateQuizzDto dtos);
        Task<QuizzDto> GetFullQuizzByIdAsync(Guid id);
        Task<QuizzInfoDto> GetQuizInfoByIdAsync(Guid id);
    }
}
