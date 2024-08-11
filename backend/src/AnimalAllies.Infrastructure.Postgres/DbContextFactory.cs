using AnimalAllies.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace AnimalAllies.Infrastructure;

public class DbContextFactory: IDesignTimeDbContextFactory<AnimalAlliesDbContext>
{
    public AnimalAlliesDbContext CreateDbContext(string[] args)
    {
        var pathToSolution = Directory.GetParent(Directory.GetCurrentDirectory());
        var path = Path.Combine(pathToSolution.FullName, "AnimalAllies.API", "appsettings.json");
        
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(path)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AnimalAlliesDbContext>();

        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            .UseLoggerFactory(AnimalAlliesDbContext.CreateLoggerFactory)
            .EnableSensitiveDataLogging();

        return new AnimalAlliesDbContext(configuration);
    }
}