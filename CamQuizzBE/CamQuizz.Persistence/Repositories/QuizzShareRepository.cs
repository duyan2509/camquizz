using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        public async Task<QuizzShare?> GetByQuizzIdGroupIdAsync(Guid quizzId, Guid groupId)
        {
            return await _dbSet
                .Include(qs => qs.Group)
                .FirstOrDefaultAsync(qs => qs.QuizzId == quizzId
                                                          && qs.GroupId == groupId);
            
        }

        public async Task<bool> UpdateVisibleAsync(QuizzShare quizzShare, bool visible)
        {
            quizzShare.IsHide=!visible;
            await UpdateAsync(quizzShare);
            return true;
        }
        public async Task<IEnumerable<QuizzShare>> GetByQuizzIdAsync(Guid quizzId)
        {
            return await _dbSet
                .Where(qs => qs.QuizzId == quizzId)
                .ToListAsync();
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<QuizzShare> quizzShares)
        {
            _dbSet.RemoveRange(quizzShares);
             var affectedRows = await _context.SaveChangesAsync(); 
            return affectedRows > 0;
        }

        public async Task<PagedResultDto<QuizzShare>> GetQuizzesByGroupIdAsync(int page, int size, Guid groupId, Guid userId)
        {
           var query = _dbSet.AsNoTracking()
               .Include(qs => qs.Quizz)
                    .ThenInclude(q => q.Questions)
               .Include(qs => qs.Quizz)
                    .ThenInclude(q => q.Genre)
               .Include(qs => qs.User)
               .Include(qs=>qs.Group)
               .Where(qs => qs.GroupId == groupId
                            &&(
                                !qs.IsHide
                                || qs.UserId == userId
                                || qs.Group.OwnerId == userId
                            ));
               
          

            int count = await query.CountAsync();

            var quizzes = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
            return new PagedResultDto<QuizzShare>
            {
                Data = quizzes,
                Page = page,
                Size = size,
                Total = count
            };
        }
    }
}
