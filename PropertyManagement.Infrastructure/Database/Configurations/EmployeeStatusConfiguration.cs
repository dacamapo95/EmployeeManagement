using EmployeeManagement.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Infrastructure.Database.Configurations;

public class EmployeeStatusConfiguration : IEntityTypeConfiguration<EmployeeStatus>
{
    public void Configure(EntityTypeBuilder<EmployeeStatus> builder)
    {
        builder.ToTable("EmployeeStatuses");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(s => s.Name)
            .IsUnique();

        builder.HasMany(s => s.Employees)
            .WithOne(e => e.Status)
            .HasForeignKey(e => e.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new EmployeeStatus { Id = (int)EmployeeStatusEnum.Active, Name = "Active" },
            new EmployeeStatus { Id = (int)EmployeeStatusEnum.Inactive, Name = "Inactive" },
            new EmployeeStatus { Id = (int)EmployeeStatusEnum.Suspended, Name = "Suspended" },
            new EmployeeStatus { Id = (int)EmployeeStatusEnum.Terminated, Name = "Terminated" }
        );
    }
}
