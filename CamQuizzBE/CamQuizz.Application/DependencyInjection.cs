using Microsoft.Extensions.DependencyInjection;
using CamQuizz.Application.Interfaces;
using CamQuizz.Application.Services;

namespace CamQuizz.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IAnswerService, AnswerService>();
        services.AddScoped<IQuizzService, QuizzService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<JwtService>();    

        return services;
    }
}
