using Encaixa.Infrastructure.UserIdentity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Encaixa.Infrastructure.Context;
public partial class EncaixaContext(DbContextOptions<EncaixaContext> options)
: IdentityDbContext<UserApplication>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUsers(modelBuilder);
        ConfigurePackageBoxes(modelBuilder);
    }
}
