using EmployeeManagement.Shared.Primitives;

namespace EmployeeManagement.Domain.Employees;

public sealed class Role : MasterEntity<int>
{
    public ICollection<Employee> Employees { get; set; } = [];
}
