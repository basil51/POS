using POS.Application.Abstractions;

namespace POS.Wpf.Services;

public sealed class CurrentSession : ICurrentSession
{
    public Guid UserId { get; private set; }
    public Guid StoreId { get; private set; }
    public string Username { get; private set; } = "";
    public string RoleName { get; private set; } = "";
    public bool IsAuthenticated { get; private set; }

    public void Set(Guid userId, Guid storeId, string username, string roleName)
    {
        UserId = userId;
        StoreId = storeId;
        Username = username;
        RoleName = roleName;
        IsAuthenticated = true;
    }

    public void Clear()
    {
        UserId = Guid.Empty;
        StoreId = Guid.Empty;
        Username = "";
        RoleName = "";
        IsAuthenticated = false;
    }
}
