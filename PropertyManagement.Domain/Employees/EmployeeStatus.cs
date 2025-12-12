using EmployeeManagement.Shared.Primitives;

namespace EmployeeManagement.Domain.Employees;

public sealed class EmployeeStatus : MasterEntity<int>
{
    public ICollection<Employee> Employees { get; set; } = [];
}
