using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Domain.Companies;

public interface ICompanyRepository : IReadRepository<Company, int>
{
}
