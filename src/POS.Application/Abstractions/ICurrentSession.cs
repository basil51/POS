namespace POS.Application.Abstractions;

public interface ICurrentSession
{
    Guid UserId { get; }
    Guid StoreId { get; }
    string Username { get; }
    string RoleName { get; }
    bool IsAuthenticated { get; }

    void Set(Guid userId, Guid storeId, string username, string roleName);
    void Clear();
}
