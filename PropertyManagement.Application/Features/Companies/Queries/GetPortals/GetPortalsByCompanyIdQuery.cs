using EmployeeManagement.Application.Core.Abstractions;

namespace EmployeeManagement.Application.Features.Companies.Queries.GetPortals;

public sealed record GetPortalsByCompanyIdQuery(int CompanyId) : IQuery<IReadOnlyList<PortalResponse>>;

public sealed record PortalResponse(int Id, string Name, int CompanyId);
