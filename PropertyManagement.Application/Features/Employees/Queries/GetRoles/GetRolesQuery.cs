using EmployeeManagement.Application.Core.Abstractions;

namespace EmployeeManagement.Application.Features.Employees.Queries.GetRoles;

public sealed record GetRolesQuery : IQuery<IReadOnlyList<RoleResponse>>;

public sealed record RoleResponse(int Id, string Name);
