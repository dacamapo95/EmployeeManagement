using EmployeeManagement.Application.Core.Abstractions;

namespace EmployeeManagement.Application.Features.Employees.Commands.Create;

public sealed record CreateEmployeeCommand(
    int CompanyId,
    int PortalId,
    int RoleId,
    string Username,
    string Email,
    string Password,
    string Name,
    string? Telephone,
    string? Fax
) : ICommand<CreateEmployeeResponse>;
