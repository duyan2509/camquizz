using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Repositories;

namespace CamQuizz.Persistence.Interfaces
{
    public interface IQuizzShareRepository : IGenericRepository<QuizzShare>
    {
        Task<IEnumerable<QuizzShare>> GetByUserIdGroupIdAsync(Guid userId, Guid groupId);
        Task<bool> DeleteRangeAsync(IEnumerable<QuizzShare> quizzShares);
    }
}
