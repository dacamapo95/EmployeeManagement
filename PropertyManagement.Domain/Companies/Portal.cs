using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Shared.Primitives;

namespace EmployeeManagement.Domain.Companies;

public sealed class Portal : MasterEntity<int>
{
    public int CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public string Name { get; set; } = default!;

    public ICollection<Employee> Employees { get; set; } = [];
}
