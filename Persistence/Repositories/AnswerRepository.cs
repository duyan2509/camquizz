using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Interfaces;

namespace CamQuizz.Persistence.Repositories
{
    public class AnswerRepository : GenericRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(ApplicationDbContext context, ILogger<Answer> logger)
        : base(context, logger)

        {
        }

        public async Task<List<Answer>> AddRangeAsync(List<Answer> answers)
        {
            await _dbSet.AddRangeAsync(answers);
            await _context.SaveChangesAsync();
            return answers;
        }
        public async Task<bool> DeleteRangeAsync(List<Answer> answers)
        {
            _dbSet.RemoveRange(answers);
            await _context.SaveChangesAsync();
            return true;    
        }
        public async Task<List<Answer>> UpdateRangeAsync(List<Answer> answers)
        {
            _dbSet.UpdateRange(answers);
            await _context.SaveChangesAsync();
            return answers;
        }
    }
}
