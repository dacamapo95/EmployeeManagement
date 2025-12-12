using EmployeeManagement.Application.Core.Abstractions;

namespace EmployeeManagement.Application.Features.Employees.Queries.GetById;

public sealed record GetEmployeeByIdQuery(Guid Id) : IQuery<EmployeeResponse>;
