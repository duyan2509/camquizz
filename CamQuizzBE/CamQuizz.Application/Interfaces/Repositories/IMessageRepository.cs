using CamQuizz.Application.Dtos;
using CamQuizz.Application.Interfaces;
using CamQuizz.Domain.Entities;

namespace CamQuizz.Application.Interfaces;

public interface IMessageRepository:IGenericRepository<GroupMessage>
{
    Task<PagedResultDto<GroupMessage>> GetGroupMessageAsync(Guid groupId, int page, int size);
    Task<GroupMessage?> GetUserMessageAsync(Guid messageId);
    Task<IEnumerable<GroupMessage>> GetGroupMessageAsync(Guid groupId);

}