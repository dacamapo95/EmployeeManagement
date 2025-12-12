using EmployeeManagement.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Infrastructure.Database.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.Property(e => e.Username)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Password)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Telephone)
            .HasMaxLength(50);

        builder.Property(e => e.Fax)
            .HasMaxLength(50);

        builder.Property(e => e.CompanyId)
            .IsRequired();

        builder.Property(e => e.PortalId)
            .IsRequired();

        builder.Property(e => e.RoleId)
            .IsRequired()
            .HasDefaultValue((int)RoleEnum.Employee);

        builder.Property(e => e.StatusId)
            .IsRequired()
            .HasDefaultValue((int)EmployeeStatusEnum.Active);

        builder.Property(e => e.CreatedAtUtc)
            .IsRequired();

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(200);

        builder.Property(e => e.LastModifiedAtUtc);

        builder.Property(e => e.LastModifiedBy)
            .HasMaxLength(200);

        builder.Property(e => e.LastLogin);

        builder.Property(e => e.DeletedOn);

        builder.HasIndex(e => e.Username)
            .IsUnique();

        builder.HasIndex(e => e.Email)
            .IsUnique();

        builder.HasOne(e => e.Company)
            .WithMany(c => c.Employees)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Portal)
            .WithMany(p => p.Employees)
            .HasForeignKey(e => e.PortalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Role)
            .WithMany(r => r.Employees)
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Status)
            .WithMany(s => s.Employees)
            .HasForeignKey(e => e.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
