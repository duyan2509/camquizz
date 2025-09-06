using CamQuizz.Application.Interfaces;
using CamQuizz.Infrastructure.Cloudinary;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CamQuizz.Infrastructure.Redis;

public static class DependencyInjection
{
    public static IServiceCollection AddRedisInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis") 
                                  ?? configuration["Redis:ConnectionString"];

        if (redisConnectionString != null)
        {
            var multiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        }

        services.AddSingleton<IQuizSessionCache, RedisQuizSessionCache>();

        return services;
    }  
}