using CamQuizz.Domain.Entities;
namespace CamQuizz.Application.Interfaces
{
    public interface IAnswerRepository : IGenericRepository<Answer>
    {
        Task<List<Answer>> AddRangeAsync(List<Answer> answers);
        Task<bool> DeleteRangeAsync(List<Answer> answers);
        Task<List<Answer>> UpdateRangeAsync(List<Answer> answers);
        
    }
}
