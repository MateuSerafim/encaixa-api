namespace Encaixa.Infrastructure.UserIdentity;
public class UserReference(Guid userId = default)
{
    public Guid UserId { get; set; } = userId;
}
