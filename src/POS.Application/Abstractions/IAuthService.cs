namespace POS.Application.Abstractions;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
}

public sealed record AuthResult(bool Success, string? ErrorMessage);
