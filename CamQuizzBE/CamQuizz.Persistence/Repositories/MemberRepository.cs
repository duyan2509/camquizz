using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain.Entities;
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
                .Include(member=>member.Group)
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

        public Task<List<UserGroup>> GetAllReceiversAsync(Guid groupId, Guid userId)
        {
            var members =  _dbSet
                .Include(ug => ug.User)
                .Where(member => member.GroupId == groupId && member.UserId!=userId).ToList();
            return Task.FromResult(members);
        }

        public Task<IEnumerable<UserGroup>> GetAllMembersAsync(Guid groupId)
        {
            var members =  _dbSet
                .Include(ug => ug.User)
                .Where(member => member.GroupId == groupId).ToList();
            return Task.FromResult<IEnumerable<UserGroup>>(members);
        }

        public async Task<PagedResultDto<ConversationPreview>> GetAllConversationsAsync(int page, int size, Guid userId)
        {
            var query = _dbSet
                .Where(ug => userId == ug.UserId)
                .Select(ug=>new 
                {
                    GroupId = ug.GroupId,
                    GroupName = ug.Group.Name,
                    LastMessage = ug.Group.GroupMessages
                        .OrderByDescending(m=>m.CreatedAt)
                        .Select(m=>new
                        {
                            Message= m.Message,
                            SenderName = $"{m.User.LastName}",
                            CreatedAt = m.CreatedAt,
                            Id = m.Id
                        })
                        .FirstOrDefault(),
                    UnreadCount = ug.Group.GroupMessages.Count(m=>m.CreatedAt>ug.LastReadMessage.CreatedAt)
                });
            
            int count = await query.CountAsync();
            var data = await query
                .OrderByDescending(x => x.LastMessage!.CreatedAt)
                .Skip((page - 1) * size).Take(size).ToListAsync();
            var conversations = data.Select(x => new ConversationPreview
                {
                    GroupId = x.GroupId,
                    GroupName = x.GroupName,
                    LastMessageAt = x.LastMessage?.CreatedAt,
                    SenderName = x.LastMessage?.SenderName ?? "(Chưa có tin nhắn)",
                    UnreadCount = x.UnreadCount,
                    LastMessage = x.LastMessage?.Message ?? "Hãy gửi tin nhắn đầu tiên",
                    LastMessageId = x.LastMessage?.Id ?? null
                })
                .ToList();
            
            return new PagedResultDto<ConversationPreview>
            {
                Data = conversations,
                Page = page,
                Size = size,
                Total = count,
            };
        }
    }
}
