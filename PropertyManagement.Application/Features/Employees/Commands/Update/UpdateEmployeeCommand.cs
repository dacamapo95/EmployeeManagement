using EmployeeManagement.Application.Core.Abstractions;
using System.Text.Json.Serialization;

namespace EmployeeManagement.Application.Features.Employees.Commands.Update;

public sealed record UpdateEmployeeCommand(

    [property: JsonIgnore]Guid Id,
    int CompanyId,
    int PortalId,
    int RoleId,
    string Name,
    string Email,
    string? Telephone,
    string? Fax,
    int StatusId
) : ICommand;
