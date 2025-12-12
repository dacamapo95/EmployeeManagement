using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Shared.Primitives;

namespace EmployeeManagement.Domain.Companies;

public sealed class Company : MasterEntity<int>
{
    public ICollection<Portal> Portals { get; set; } = [];
    public ICollection<Employee> Employees { get; set; } = [];
}
