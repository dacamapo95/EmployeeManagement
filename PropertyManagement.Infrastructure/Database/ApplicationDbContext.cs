using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Infrastructure;
using EmployeeManagement.Domain.Companies;
using EmployeeManagement.Domain.Employees;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Portal> Portals => Set<Portal>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<EmployeeStatus> EmployeeStatuses => Set<EmployeeStatus>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("EMY");

        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}