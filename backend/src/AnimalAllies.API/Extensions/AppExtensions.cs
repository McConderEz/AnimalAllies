using AnimalAllies.Infrastructure;
using AnimalAllies.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace AnimalAllies.API.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigrations(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}