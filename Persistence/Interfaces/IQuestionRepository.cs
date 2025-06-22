using CamQuizz.Application.Dtos;
using CamQuizz.Domain.Entities;
using CamQuizz.Persistence.Repositories;

namespace CamQuizz.Persistence.Interfaces
{
    public interface IQuestionRepository: IGenericRepository<Question>
    {
    }
}
