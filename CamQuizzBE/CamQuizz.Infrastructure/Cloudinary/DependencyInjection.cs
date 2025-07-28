using CamQuizz.Application.Interfaces;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CamQuizz.Infrastructure.Cloudinary;

public static class DependencyInjection
{
    public static IServiceCollection AddCloudinaryInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new CloudinaryOptions();
        configuration.GetSection("CloudinarySettings").Bind(settings);

        var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
        Console.WriteLine(account);
        var cloudinary = new CloudinaryDotNet.Cloudinary(account);

        services.AddSingleton(cloudinary);
        services.AddSingleton<ICloudStorageService, CloudinaryStorageService>();

        return services;
    }  
}