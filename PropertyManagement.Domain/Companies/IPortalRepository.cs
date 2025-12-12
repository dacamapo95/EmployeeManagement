using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Domain.Companies;

public interface IPortalRepository  : IReadRepository<Portal, int>
{
    Task<Portal?> GetByIdWithCompanyAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Portal>> GetByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}
