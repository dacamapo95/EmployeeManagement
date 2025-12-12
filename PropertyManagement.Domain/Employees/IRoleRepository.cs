using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Domain.Employees;

public interface IRoleRepository : IReadRepository<Role, int>
{
}
