using Microsoft.EntityFrameworkCore;
using POS.Application.Abstractions;
using POS.Infrastructure.Data;

namespace POS.Infrastructure.Services;

internal sealed class AuthService : IAuthService
{
    private readonly IDbContextFactory<PosDbContext> _dbFactory;
    private readonly ICurrentSession _session;

    public AuthService(IDbContextFactory<PosDbContext> dbFactory, ICurrentSession session)
    {
        _dbFactory = dbFactory;
        _session = session;
    }

    public async Task<AuthResult> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return new AuthResult(false, "Username and password are required.");

        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);
        var user = await db.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username.Trim() && !u.IsDeleted, cancellationToken);

        if (user is null || !user.IsActive)
            return new AuthResult(false, "Invalid username or password.");

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return new AuthResult(false, "Invalid username or password.");

        _session.Set(user.Id, user.StoreId, user.Username, user.Role?.Name ?? "");

        return new AuthResult(true, null);
    }
}
