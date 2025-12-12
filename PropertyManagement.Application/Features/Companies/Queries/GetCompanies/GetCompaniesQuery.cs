using EmployeeManagement.Application.Core.Abstractions;

namespace EmployeeManagement.Application.Features.Companies.Queries.GetCompanies;

public sealed record GetCompaniesQuery : IQuery<IReadOnlyList<CompanyResponse>>;

public sealed record CompanyResponse(int Id, string Name);
