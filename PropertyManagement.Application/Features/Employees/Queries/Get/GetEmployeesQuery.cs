using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Shared.Pagination;

namespace EmployeeManagement.Application.Features.Employees.Queries.Get;

public sealed record GetEmployeesQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? Search = null,
    int? CompanyId = null,
    int? PortalId = null,
    int? RoleId = null,
    int? StatusId = null,
    string? OrderBy = null,
    bool Desc = false
) : IQuery<PagedResult<EmployeeListItem>>;

public sealed record EmployeeListItem(
    Guid Id,
    string Username,
    string Email,
    string Name,
    string? Telephone,
    int CompanyId,
    string CompanyName,
    int PortalId,
    string PortalName,
    int RoleId,
    string RoleName,
    int StatusId,
    string StatusName,
    DateTime? LastLogin
);
