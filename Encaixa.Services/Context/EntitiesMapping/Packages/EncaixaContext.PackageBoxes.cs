using Encaixa.Domain.Packages;
using Encaixa.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Encaixa.Infrastructure.Context;
public partial class EncaixaContext
{
    public DbSet<PackageBox> PackageBoxes { get; set; }

    private static void ConfigurePackageBoxes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PackageBox>(e =>
        {
            e.ToTable("PackageBoxes", schema: "Package");
            e.HasKey(c => c.Id);
            e.Property(c => c.EntityStatus);

            e.Property(c => c.BoxLabel).HasMaxLength(50);

            e.Property(c => c.Height)
             .HasConversion(c => c.Value, c => Dimension.Build(c).GetValue());
            e.Property(c => c.Width)
             .HasConversion(c => c.Value, c => Dimension.Build(c).GetValue());
            e.Property(c => c.Length)
             .HasConversion(c => c.Value, c => Dimension.Build(c).GetValue());

            e.HasOne(c => c.User)
             .WithMany(c => c.PackageBoxes)
             .HasForeignKey(c => c.UserId);
        });
    }
}
