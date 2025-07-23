using CamQuizz.Application.Interfaces;
using CamQuizz.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CamQuizz.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Add DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDbConnectionFactory>(_ =>
        {
            var connStr = configuration.GetConnectionString("DefaultConnection");
            return new SqlConnectionFactory(connStr);
        });
        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IAnswerRepository, AnswerRepository>();
        services.AddScoped<IQuizzRepository, QuizzRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IQuizzShareRepository, QuizzShareRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        return services;
    }
}
