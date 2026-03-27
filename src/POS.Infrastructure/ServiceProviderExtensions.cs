using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using POS.Infrastructure.Data;

namespace POS.Infrastructure;

public static class ServiceProviderExtensions
{
    public static void ApplyPosDatabaseMigrations(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PosDbContext>();
        db.Database.Migrate();
    }
}
