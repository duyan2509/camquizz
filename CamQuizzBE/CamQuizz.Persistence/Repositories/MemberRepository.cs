using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CamQuizz.Persistence.Repositories
{
    public class MemberRepository : GenericRepository<UserGroup>, IMemberRepository
    {
        public MemberRepository(ApplicationDbContext context, ILogger<UserGroup> logger)
        : base(context, logger)
        {
        }
        public async Task<UserGroup?> GetByUserIdGroupIdAsync(Guid userId, Guid groupId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(member => member.UserId == userId && member.GroupId == groupId);
        }
        public async Task<PagedResultDto<UserGroup>> GetByGroupIdAsync(int page, int size, Guid groupId)
        {
            var query = _dbSet
                .Include(ug => ug.User)
                .Where(ug => ug.GroupId == groupId);
            var total = await query.CountAsync();
            var userGroups = await query
                            .Skip((page - 1) * size)
                            .Take(size)
                            .ToListAsync();
            return new PagedResultDto<UserGroup>
            {
                Data = userGroups,
                Total = total,
                Page = page,
                Size = size
            };
        }

        public async Task UpdateLastReadMessage(UserGroup member, Guid messageId)
        {
            member.LastReadMessageId = messageId;
            await UpdateAsync(member);
        }
    }
}
