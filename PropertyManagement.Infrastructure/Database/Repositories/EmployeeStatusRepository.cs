using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Domain.Employees;

namespace EmployeeManagement.Infrastructure.Database.Repositories;

public sealed class EmployeeStatusRepository(
    IDbConnectionFactory connectionFactory,
    ApplicationDbContext context)
    : Repository<EmployeeStatus, int>(connectionFactory, context), IEmployeeStatusRepository
{
    protected override string TableName => "EmployeeStatuses";
}
