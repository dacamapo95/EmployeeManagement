using EmployeeManagement.Application.Core.Abstractions;

namespace EmployeeManagement.Application.Features.Employees.Queries.GetStatuses;

public sealed record GetEmployeeStatusesQuery : IQuery<IReadOnlyList<EmployeeStatusResponse>>;

public sealed record EmployeeStatusResponse(int Id, string Name);
