using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CamQuizz.Persistence.Repositories
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(ApplicationDbContext context, ILogger<Question> logger)
        : base(context, logger)
        {
        }

        public Task<Question> GetFullQuestionByIdAsync(Guid questionId)
        {
            var question = _dbSet
                    .Include(question => question.Answers)
                    .FirstOrDefaultAsync(question => question.Id == questionId);
            return question;
        }

        public async Task<PagedResultDto<Question>> GetQuestionsByQuizIdAsync(Guid quizId,string? kw, bool newest, int pageNumber, int pageSize)
        {
            var query = _dbSet.AsQueryable()
                .Include(question => question.Answers)
                .Where(question => question.QuizzId == quizId);
            if(!string.IsNullOrEmpty(kw))
                query = query.Where(question=>question.Content.ToUpper().Contains(kw.ToUpper()));
            query = newest ? query.OrderBy(question => question.CreatedAt) : query.OrderByDescending(question => question.CreatedAt);
            
            int count = await query.CountAsync();
            var questions = await query.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();
            return new PagedResultDto<Question>
            {
                Data = questions,
                Total = count,
                Size = pageSize,
                Page = pageNumber,
            };
        }

        public Task<int> GetQuestionCountByQuizIdAsync(Guid quizId)
        {
            return _dbSet.AsNoTracking().CountAsync(question => question.QuizzId == quizId);
        }
    }
}
