using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Interfaces;

namespace CamQuizz.Persistence.Repositories
{
    public class AnswerRepository : GenericRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Answer>> AddRangeAsync(List<Answer> answers)
        {
            await _dbSet.AddRangeAsync(answers);
            await _context.SaveChangesAsync();
            return answers;
        }
    }
}
