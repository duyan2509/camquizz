using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

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
    }
}
