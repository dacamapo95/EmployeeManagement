using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Domain.Employees;

namespace EmployeeManagement.Infrastructure.Database.Repositories;

public sealed class RoleRepository(
    IDbConnectionFactory connectionFactory,
    ApplicationDbContext context)
    : Repository<Role, int>(connectionFactory, context), IRoleRepository
{
    protected override string TableName => "Roles";
}
