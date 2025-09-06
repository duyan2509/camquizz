using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;

namespace CamQuizz.Application.Interfaces
{
    public interface IQuizzShareRepository : IGenericRepository<QuizzShare>
    {
        Task<IEnumerable<QuizzShare>> GetByUserIdGroupIdAsync(Guid userId, Guid groupId);
        Task<QuizzShare?> GetByQuizzIdGroupIdAsync(Guid quizzId, Guid groupId);
        Task<IEnumerable<QuizzShare>?> GetByQuizzIdAsync(Guid quizzId);
        Task<IEnumerable<QuizzShare>> GetByGroupIddAsync(Guid groupId);
        
        Task<bool> DeleteRangeAsync(IEnumerable<QuizzShare> quizzShares);
        Task<bool> UpdateVisibleAsync(QuizzShare quizzShare, bool visible);
        Task<PagedResultDto<QuizzShare>> GetQuizzesByGroupIdAsync(int page, int size, string? kw, Guid groupId, Guid userId);
        Task<bool> CheckAccessAsync(Guid userId, Guid quizzId);
    }
}
