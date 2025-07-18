using CamQuizz.Application.Dtos;
using CamQuizz.Application.Exceptions;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CamQuizz.Persistence.Repositories
{
    public class GroupRepository : GenericRepository<Group>, IGroupRepository
    {
        public GroupRepository(ApplicationDbContext context, ILogger<Group> logger)
        : base(context, logger)
        {
        }
        public async Task<Group?> GetFullGroupByIdAsync(Guid id)
        {
            return await _dbSet
                    .Include(group => group.QuizzShares)
                        .ThenInclude(qs => qs.Quizz)
                    .Include(group => group.UserGroups)
                        .ThenInclude(ug => ug.User)
                    .Include(group => group.Owner)
                    .FirstOrDefaultAsync(group => group.Id == id);
        }
        public async Task<Group?> GetGroupInfoIdAsync(Guid id)
        {
            return await _dbSet
                    .Include(group => group.Owner)
                    .Include(group => group.QuizzShares)
                        .ThenInclude(qs => qs.Quizz)
                    .FirstOrDefaultAsync(group => group.Id == id);
        }
        public async Task<PagedResultDto<Group>> GetOwnerGroupsAsync(string kw, int page, int size, Guid userId)
        {
            var query = _dbSet
                .Where(group => group.OwnerId == userId && group.Name.Contains(kw))
                .Include(group => group.Owner)
                .Include(group=>group.UserGroups)
                .Include(group => group.QuizzShares);

            var total = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return new PagedResultDto<Group>
            {
                Data = data,
                Page = page,
                Size = size,
                Total = total
            };
        }
        public async Task<PagedResultDto<Group>> GetMemberGroupsAsync(string kw, int page, int size, Guid userId)
        {
            var query = _dbSet
                .Include(group => group.Owner)
                .Include(group => group.QuizzShares)
                .Include(group => group.UserGroups)
                .Where(group => group.UserGroups.Any(ug => ug.UserId == userId) && group.Name.Contains(kw))
                .Where(group => group.OwnerId != userId);

            var total = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return new PagedResultDto<Group>
            {
                Data = data,
                Page = page,
                Size = size,
                Total = total
            };
        }
        public async Task<PagedResultDto<Group>> GetAllGroupsAsync(string kw, int page, int size, Guid userId)
        {
            var query = _dbSet
                .Include(group => group.Owner)
                .Include(group => group.QuizzShares)
                .Include(group => group.UserGroups)
                .Where(group => group.UserGroups.Any(ug => ug.UserId == userId)&& group.Name.Contains(kw));

            var total = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return new PagedResultDto<Group>
            {
                Data = data,
                Page = page,
                Size = size,
                Total = total
            };
        }
        public async Task<List<Group>> GetAllGroupsAsync(Guid userId)
        {
            return await _dbSet
                .Include(group => group.Owner)
                .Include(group => group.QuizzShares)
                .Include(group => group.UserGroups)
                .Where(group => group.UserGroups.Any(ug => ug.UserId == userId))
                .ToListAsync();
        }

        public async Task<List<Group>> GetQuizzGroupsAsync(Guid quizzId, bool share)
        {
            return await _dbSet
                .Include(group => group.Owner)
                .Include(group => group.QuizzShares)
                .ThenInclude(qs => qs.Quizz)
                .Include(group => group.UserGroups)
                .Where(group =>
                    (share && group.QuizzShares.Any(qs => qs.QuizzId == quizzId))
                    || (!share && (group.QuizzShares.All(qs => qs.QuizzId != quizzId)))
                )
                .ToListAsync();
        }
        public async Task<Group> UpdateNameAsync(Group group)
        {
            try
            {
                _dbSet.Update(group);
                await _context.SaveChangesAsync();
                return group;
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is SqlException sqlEx &&
                      (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                throw new ConflictException("Group name is existed for this user.");
            }
        }

        public async Task<Group> CreateAsync(Group group)
        {
            try
            {
                await _dbSet.AddAsync(group);
                await _context.SaveChangesAsync();
                return group;
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is SqlException sqlEx &&
                      (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                throw new ConflictException("Group name is existed for this user.");
            }
        }
    }
}
