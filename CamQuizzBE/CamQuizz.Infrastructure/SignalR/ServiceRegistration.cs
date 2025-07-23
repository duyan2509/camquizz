using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace CamQuizz.Infrastructure.SignalR;

public static class ServiceRegistration
{
    public static IServiceCollection AddSignalRInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<GroupChatConnectionManager>();
        services.AddSingleton<IUserIdProvider, UserIdProvider>();
        return services;
    }
}
