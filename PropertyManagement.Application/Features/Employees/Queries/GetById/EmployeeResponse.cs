namespace EmployeeManagement.Application.Features.Employees.Queries.GetById;

public sealed record EmployeeResponse(
    Guid Id,
    int CompanyId,
    string CompanyName,
    int PortalId,
    string PortalName,
    int RoleId,
    string RoleName,
    int StatusId,
    string StatusName,
    string Username,
    string Email,
    string Name,
    string? Telephone,
    string? Fax,
    DateTime? LastLogin,
    DateTime CreatedAtUtc,
    DateTime? LastModifiedAtUtc
);
