using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CamQuizz.Persistence.Interfaces;

public interface IGenericRepository<T>
    where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task SoftDeleteAsync(Guid id);
    Task HardDeleteAsync(Guid id);

    Task<bool> ExistsAsync(Guid id);
    Task<int> CountAsync();
    Task<PagedResultDto<T>> GetPagedAsync(int pageNumber, int pageSize);
    Task<T?> GetByIdIncludingDeletedAsync(Guid id);

}
