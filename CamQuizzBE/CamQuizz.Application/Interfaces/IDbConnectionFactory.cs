using System.Data;

namespace CamQuizz.Application.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}

