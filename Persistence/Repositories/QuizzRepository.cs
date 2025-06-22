using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CamQuizz.Persistence.Repositories
{
    public class QuizzRepository : GenericRepository<Quizz>, IQuizzRepository
    {
        public QuizzRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<Quizz?> GetFullByIdAsync(Guid id)
        {
            return _dbSet
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .Include(q => q.Author)
                .Include(q => q.Genre)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public Task<Quizz?> GetInfoByIdAsync(Guid id)
        {
            return _dbSet
                .Include(q=>q.Author)
                .Include(q=>q.Genre)
                .Include(q=>q.Questions)
                .ThenInclude(q=>q.Answers)
                .FirstOrDefaultAsync(q=>q.Id == id);
        }
    }
}
