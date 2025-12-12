using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Domain.Employees;

public interface IEmployeeRepository : IRepository<Employee, Guid>
{
    Task<Employee?> GetDetailsByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Employee?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

    Task<Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> IsUsernameUniqueAsync(string username, Guid? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Employee> Items, int TotalCount)> SearchAsync(
        int pageNumber,
        int pageSize,
        string? search,
        int? companyId,
        int? portalId,
        int? roleId,
        int? statusId,
        string? orderBy,
        bool desc,
        CancellationToken cancellationToken = default);
}
