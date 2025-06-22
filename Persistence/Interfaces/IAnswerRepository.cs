using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Repositories;
namespace CamQuizz.Persistence.Interfaces
{
    public interface IAnswerRepository: IGenericRepository<Answer>
    {
        Task<ICollection<Answer>> AddRangeAsync(List<Answer> answers);
    }
}
