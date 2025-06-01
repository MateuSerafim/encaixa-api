using Encaixa.Domain.Users;
using Encaixa.Domain.ValueObjects;
using Encaixa.Infrastructure.UserIdentity;
using Microsoft.EntityFrameworkCore;

namespace Encaixa.Infrastructure.Context;
public partial class EncaixaContext
{
    public DbSet<User> UsersDomain { get; set; }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("[User].UsersData");
            e.HasKey(e => e.Id);
            e.Property(c => c.EntityStatus);

            e.Property(c => c.FirstName)
             .HasConversion(c => c.Value,
                            c => Name.Build(c).GetValue())
             .HasMaxLength(Name.MaxNameLength);
            e.Property(c => c.Surname)
             .HasConversion(c => c != null ? c.Value : null,
                            c => string.IsNullOrEmpty(c) ? null :
                                 Name.Build(c).GetValue())
             .HasMaxLength(Name.MaxNameLength)
             .IsRequired(false);
            e.Property(c => c.Email)
             .HasConversion(c => c.Value,
                            c => Email.Build(c).GetValue())
             .HasMaxLength(Email.MaxEmailLength);
        });

        modelBuilder.Entity<UserApplication>()
        .HasOne(a => a.User).WithOne()
        .HasForeignKey<UserApplication>(f => f.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
