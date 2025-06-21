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
        
        services.AddSingleton<JwtService>();

        return services;
    }
}
