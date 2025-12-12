using EmployeeManagement.Shared.Results;

namespace EmployeeManagement.Domain.Employees;

public static class EmployeeErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound($"Employee with ID {id} was not found.");

    public static Error UsernameAlreadyExists(string username) =>
        Error.Conflict($"Username '{username}' is already in use.");

    public static Error EmailAlreadyExists(string email) =>
        Error.Conflict($"Email '{email}' is already in use.");

    public static Error PortalNotInCompany() =>
        Error.Validation("Portal does not belong to the specified company.");

    public static Error CompanyNotFound(int id) =>
        Error.NotFound($"Company with ID {id} was not found.");

    public static Error PortalNotFound(int id) =>
        Error.NotFound($"Portal with ID {id} was not found.");
}
