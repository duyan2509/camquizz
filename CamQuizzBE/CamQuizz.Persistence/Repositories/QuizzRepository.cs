using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain.Entities;
using CamQuizz.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CamQuizz.Persistence.Repositories
{
    public class QuizzRepository : GenericRepository<Quizz>, IQuizzRepository
    {
        public QuizzRepository(ApplicationDbContext context, ILogger<Quizz> logger)
        : base(context, logger)
        {
        }

        public Task<Quizz?> GetDetailByIdAsync(Guid id)
        {
            return _dbSet
                .Include(q => q.Genre)
                .Include(q => q.QuizzShares)
                .ThenInclude(qs => qs.Group)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public Task<Quizz?> GetFullByIdAsync(Guid id)
        {
            return _dbSet
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .Include(q => q.Author)
                .Include(q => q.Genre)
                .Include(q=>q.QuizzShares)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public Task<Quizz?> GetAccessQuizzByIdAsync(Guid id)
        {
            return _dbSet
                .Include(q => q.Author)
                .Include(q => q.QuizzShares)
                .ThenInclude(qs => qs.Group)
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


        public async Task<Quizz> UpdateStatusASync(Quizz quizz, QuizzStatus newStatus)
        {
            if(quizz.Status != newStatus)
                quizz.Status = newStatus;
            await UpdateAsync(quizz);
            return quizz;
        }
        
        public async Task<PagedResultDto<Quizz>> GetAllQuizzesAsync(    
            string? kw,
            Guid? categoryId,
            bool popular,
            bool newest,
            int pageNumber,
            int pageSize)
        {
            var query = _dbSet
                .Include(q => q.Questions)
                .Include(q=>q.Genre)                
                .Where(q => q.Status==QuizzStatus.Public && q.Questions.Count>0 );
            if (!string.IsNullOrWhiteSpace(kw))
                query = query.Where(q => q.Name.ToLower().Contains(kw.ToLower()));
            if(categoryId != null)
                query = query.Where(q=>q.GenreId==categoryId);
            if (popular && newest)
            {
                query = query
                    .OrderByDescending(q => q.CreatedAt)
                    .ThenByDescending(q => q.NumberOfAttemps);
            }
            else if (newest)
            {
                query = query.OrderByDescending(q => q.CreatedAt);
            }
            else if (popular)
            {
                query = query.OrderByDescending(q => q.NumberOfAttemps);
            }
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            _logger.LogInformation("Quiz Count: {Count} ", items.Count);
            
            return new PagedResultDto<Quizz>
            {
                Data = items,
                Total = totalCount,
                Page = pageNumber,
                Size = pageSize
            };
        }

        public async Task<PagedResultDto<Quizz>> GetMyQuizzesAsync(
            string? kw,
            Guid? categoryId,
            bool popular,
            bool newest,
            int pageNumber,
            int pageSize,
            QuizzStatus? quizzStatus,
            Guid userId)
        {

            var query = _dbSet
                .Include(q => q.Questions)
                .Include(q=>q.Genre)                
                .Where(q => q.AuthorId == userId );
            if (!string.IsNullOrWhiteSpace(kw))
                query = query.Where(q => q.Name.ToLower().Contains(kw.ToLower()));
            if(categoryId != null)
                query = query.Where(q=>q.GenreId==categoryId);
            if (popular && newest)
            {
                query = query
                    .OrderByDescending(q => q.CreatedAt)
                    .ThenByDescending(q => q.NumberOfAttemps);
            }
            else if (newest)
            {
                query = query.OrderByDescending(q => q.CreatedAt);
            }
            else if (popular)
            {
                query = query.OrderByDescending(q => q.NumberOfAttemps);
            }
            if (quizzStatus != null)
                query = query.Where(q => q.Status == quizzStatus);
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            _logger.LogInformation("Quiz Count: {Count} ", items.Count);
            
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
