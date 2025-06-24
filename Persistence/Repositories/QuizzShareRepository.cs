using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CamQuizz.Persistence.Repositories
{
    public class QuizzShareRepository : GenericRepository<QuizzShare>, IQuizzShareRepository
    {
        public QuizzShareRepository(ApplicationDbContext context, ILogger<QuizzShare> logger)
        : base(context, logger)
        {
        }
        public async Task<IEnumerable<QuizzShare>> GetByUserIdGroupIdAsync(Guid userId, Guid groupId)
        {
            return await _dbSet.Where(
                qs => qs.UserId == userId
                && qs.GroupId == groupId)
                .ToListAsync();
        }
        public async Task<bool> DeleteRangeAsync(IEnumerable<QuizzShare> quizzShares)
        {
            _dbSet.RemoveRange(quizzShares);
             var affectedRows = await _context.SaveChangesAsync(); 
            return affectedRows > 0;
        }

    }
}
