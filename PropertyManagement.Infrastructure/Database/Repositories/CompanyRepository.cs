using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Domain.Companies;
using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Infrastructure.Database.Repositories;

public sealed class CompanyRepository(
    IDbConnectionFactory connectionFactory)
    : ReadRepository<Company, int>(connectionFactory), ICompanyRepository
{
    protected override string TableName => "Companies";
}
