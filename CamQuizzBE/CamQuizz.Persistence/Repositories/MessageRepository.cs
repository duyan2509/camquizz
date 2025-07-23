using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain.Entities;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CamQuizz.Persistence.Repositories;
public class MessageRepository : GenericRepository<GroupMessage>, IMessageRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    public MessageRepository(ApplicationDbContext context, ILogger<GroupMessage> logger, IDbConnectionFactory connectionFactory)
        : base(context, logger)

    {
        _connectionFactory = connectionFactory;
    }


    public async Task<PagedResultDto<GroupMessage>> GetGroupMessageAsync(Guid groupId, DateTime? afterCreatedAt, Guid? afterId, int size)
    {
        using var connection = _connectionFactory.CreateConnection();
        var firstSql=@"SELECT TOP (@size)
                GM.* , 
                U.Id AS UserId, U.FirstName , U.LastName, 
                G.Id AS GroupId, G.Name
            FROM GroupMessages GM
            JOIN Users U ON GM.UserId = U.Id
            JOIN Groups G ON GM.GroupId = G.Id
            where GM.GroupId = @groupId
            ORDER BY GM.CreatedAt DESC, GM.Id DESC";
        
        var pageSql =@"SELECT TOP (@size)
                GM.* , 
                U.Id AS UserId, U.FirstName , U.LastName, 
                G.Id AS GroupId, G.Name
            FROM GroupMessages GM
            JOIN Users U ON GM.UserId = U.Id
            JOIN Groups G ON GM.GroupId = G.Id
            WHERE
            GM.GroupId = @groupId AND
            (
                GM.CreatedAt < @afterCreatedAt OR 
                (GM.CreatedAt = @afterCreatedAt AND GM.Id < @afterId)
            )
            ORDER BY GM.CreatedAt DESC, GM.Id DESC";
        var countSql = @"SELECT COUNT(*) FROM GroupMessages GM WHERE GM.GroupId = @groupId";
        var count = await connection.QueryFirstAsync<int>(countSql, new { groupId });
        var result = await connection.QueryAsync<GroupMessage, User, Group, GroupMessage>(
            afterCreatedAt!=null?pageSql:firstSql,
            (gm, user, group) =>
            {
                gm.User = user;
                gm.Group = group;
                return gm;
            },
            new { size, groupId, afterCreatedAt, afterId},
            splitOn: "UserId,GroupId"
        );
        
        return new PagedResultDto<GroupMessage>
        {
            Data = result.Reverse().ToList(),
            Total = count,
            Page = 0,
            Size = size,
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
