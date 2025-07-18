using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CamQuizz.Persistence.Repositories;
public class MessageRepository : GenericRepository<GroupMessage>, IMessageRepository
{
    public MessageRepository(ApplicationDbContext context, ILogger<GroupMessage> logger)
        : base(context, logger)

    {
    }


    public async Task<PagedResultDto<GroupMessage>> GetGroupMessageAsync(Guid groupId, int page, int size)
    {
        var query =  _dbSet.AsNoTracking()
            .Include(m=>m.User)
            .Where(x => x.GroupId == groupId)
            .OrderByDescending(x=>x.CreatedAt);
        var total = await query.CountAsync();
        var messages = await query
            .Skip(size * (page - 1))
            .Take(size)
            .ToListAsync();
        return new PagedResultDto<GroupMessage>
        {
            Data = messages,
            Total = total,
            Page = page,
            Size = size
        };
    }

    public async Task<GroupMessage?> GetUserMessageAsync(Guid messageId)
    {
        return await _dbSet
            .Include(m => m.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == messageId);
    }

    public async Task<IEnumerable<GroupMessage>> GetGroupMessageAsync(Guid groupId)
    {
        return await _dbSet.AsNoTracking()
            .Where(message=>message.GroupId == groupId)
            .ToListAsync();
    }
}    
