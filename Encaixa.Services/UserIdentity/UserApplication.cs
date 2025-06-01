using Encaixa.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Encaixa.Infrastructure.UserIdentity;

public class UserApplication : IdentityUser
{
    public Guid UserId { get; }
    public virtual User User { get; }

    public UserApplication()
    {
        
    }
    public UserApplication(User user)
    {
        User = user;
        UserId = user.Id;
        Email = user.Email.Value;
        UserName = user.Email.Value;
    }
}
