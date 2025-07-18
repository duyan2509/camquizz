using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Repositories;
namespace CamQuizz.Persistence.Interfaces
{
    public interface IAnswerRepository : IGenericRepository<Answer>
    {
        Task<List<Answer>> AddRangeAsync(List<Answer> answers);
        Task<bool> DeleteRangeAsync(List<Answer> answers);
        Task<List<Answer>> UpdateRangeAsync(List<Answer> answers);
        
    }
}
