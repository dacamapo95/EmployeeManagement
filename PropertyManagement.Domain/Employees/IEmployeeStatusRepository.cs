using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Domain.Employees;

public interface IEmployeeStatusRepository : IRepository<EmployeeStatus, int>
{
}
