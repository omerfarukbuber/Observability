using Observability.Shared;

namespace Observability.Domain.Users;

public sealed class User : Entity
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public string PasswordHash { get; set; } = null!;

}