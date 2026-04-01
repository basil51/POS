using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS.Application.Abstractions;
using POS.Infrastructure.Data;
using POS.Infrastructure.Services;

namespace POS.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers EF Core. SQLite relative paths are resolved under <paramref name="applicationBasePath"/> (use the app folder so
    /// <c>pos.db</c> is always next to the executable, regardless of the process current directory when using <c>dotnet run</c>).
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        string? applicationBasePath = null)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' is not configured.");

        var basePath = applicationBasePath ?? AppContext.BaseDirectory;
        connectionString = ResolveSqliteDataSource(connectionString, basePath);

        services.AddDbContextFactory<PosDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProductCatalogService, ProductCatalogService>();
        services.AddScoped<ISaleService, SaleService>();

        return services;
    }

    private static string ResolveSqliteDataSource(string connectionString, string basePath)
    {
        const string prefix = "Data Source=";
        if (!connectionString.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            return connectionString;

        var pathPart = connectionString[prefix.Length..].Trim();
        if (pathPart.Length == 0)
            return connectionString;

        if (pathPart.StartsWith(':') || pathPart.Equals(":memory:", StringComparison.OrdinalIgnoreCase))
            return connectionString;

        if (Path.IsPathRooted(pathPart))
            return connectionString;

        var fullPath = Path.GetFullPath(Path.Combine(basePath, pathPart));
        return $"{prefix}{fullPath}";
    }
}
