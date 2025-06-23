using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;
using CamQuizz.Domain;
using CamQuizz.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CamQuizz.Persistence.Repositories
{
    public class QuizzRepository : GenericRepository<Quizz>, IQuizzRepository
    {
        public QuizzRepository(ApplicationDbContext context, ILogger<Quizz> logger)
        : base(context, logger)
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
                .Include(q => q.Author)
                .Include(q => q.Genre)
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }
        public async Task<PagedResultDto<Quizz>> GetMyQuizzesAsync(
            int pageNumber, 
            int pageSize, 
            QuizzStatus? quizzStatus, 
            Guid userId)
        {
            _logger.LogInformation("Page Number: {PageNumber} PageSize: {PageSize} QuizzStatus: {QuizzStatus} UserId: {UserId} ", pageNumber, pageSize, quizzStatus, userId);

            var query = _dbSet.Where(q => q.AuthorId == userId);

            if (quizzStatus != null)
            {
                query = query.Where(q => q.Status == quizzStatus);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Include(q => q.Questions)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            _logger.LogInformation("Quiz Count: {Count} ", items.Count);

            foreach (var item in items)
            {
                _logger.LogInformation("Quiz: {Id} - {Name} - Status: {Status} - QuestionCount: {QuestionCount}", item.Id, item.Name, item.Status, item.Questions.Count);
            }

            return new PagedResultDto<Quizz>
            {
                Data = items,
                Total = totalCount,
                Page = pageNumber,
                Size = pageSize
            };
        }

    }
}
