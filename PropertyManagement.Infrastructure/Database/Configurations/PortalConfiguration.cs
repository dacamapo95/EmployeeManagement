using EmployeeManagement.Domain.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Infrastructure.Database.Configurations;

public class PortalConfiguration : IEntityTypeConfiguration<Portal>
{
    public void Configure(EntityTypeBuilder<Portal> builder)
    {
        builder.ToTable("Portals");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.CompanyId)
            .IsRequired();

        builder.HasIndex(p => new { p.CompanyId, p.Name })
            .IsUnique();

        builder.HasOne(p => p.Company)
            .WithMany(c => c.Portals)
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Employees)
            .WithOne(e => e.Portal)
            .HasForeignKey(e => e.PortalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
