using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace CamQuizz.Persistence;
// for design time
public class ApplicationDbContextFactory
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) 
            .AddJsonFile("appsettings.json")              
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}