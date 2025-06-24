using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace CamQuizz.Persistence.Repositories
{
    public class GroupRepository : GenericRepository<Group>, IGroupRepository
    {
        public GroupRepository(ApplicationDbContext context, ILogger<Group> logger)
        : base(context, logger)
        {
        }
        public async Task<Group> GetFullGroupByIdAsync(Guid id)
        {
            return await _dbSet
                    .Include(group => group.QuizzShares)
                        .ThenInclude(qs => qs.Quizz)
                    .Include(group => group.UserGroups)
                        .ThenInclude(ug => ug.User)
                    .Include(group => group.Owner)
                    .FirstOrDefaultAsync(group => group.Id == id);
        }
        public async Task<Group> GetGroupInfoIdAsync(Guid id)
        {
            return await _dbSet
                    .Include(group => group.Owner)
                    .Include(group => group.QuizzShares)
                        .ThenInclude(qs => qs.Quizz)
                    .FirstOrDefaultAsync(group => group.Id == id);
        }
        public async Task<PagedResultDto<Group>> GetOwnerGroupsAsync(int page, int size, Guid userId)
        {
            var query = _dbSet
                .Where(group => group.OwnerId == userId)
                .Include(group => group.Owner)
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
        public async Task<PagedResultDto<Group>> GetMemberGroupsAsync(int page, int size, Guid userId)
        {
            var query = _dbSet
                .Include(group => group.Owner)
                .Include(group => group.QuizzShares)
                .Include(group => group.UserGroups)
                .Where(group => group.UserGroups.Any(ug => ug.UserId == userId))
                .Where(group => group.OwnerId!=userId);

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
        public async Task<PagedResultDto<Group>> GetAllGroupsAsync(int page, int size, Guid userId)
        {
            var query = _dbSet
                .Include(group => group.Owner)
                .Include(group => group.QuizzShares)
                .Include(group => group.UserGroups)
                .Where(group => group.UserGroups.Any(ug => ug.UserId == userId));
                
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
    }
}
