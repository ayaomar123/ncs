using NCS.Domain.Common;

namespace NCS.Domain.Entities;

public class AdminUser : Entity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
