using Microsoft.EntityFrameworkCore;
using Observability.Infrastructure.Database;

namespace Observability.Api.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}