using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Shared.Pagination;
using EmployeeManagement.Shared.Results;

namespace EmployeeManagement.Application.Features.Employees.Queries.Get;

public sealed class GetEmployeesQueryHandler(IEmployeeRepository employeeRepository)
    : IQueryHandler<GetEmployeesQuery, PagedResult<EmployeeListItem>>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;

    public async Task<Result<PagedResult<EmployeeListItem>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var (employees, totalCount) = await _employeeRepository.SearchAsync(
            request.PageNumber,
            request.PageSize,
            request.Search,
            request.CompanyId,
            request.PortalId,
            request.RoleId,
            request.StatusId,
            request.OrderBy,
            request.Desc,
            cancellationToken);

        var items = employees.Select(e => new EmployeeListItem(
            e.Id,
            e.Username,
            e.Email,
            e.Name,
            e.Telephone,
            e.CompanyId,
            e.Company.Name,
            e.PortalId,
            e.Portal.Name,
            e.RoleId,
            e.Role.Name,
            e.StatusId,
            e.Status.Name,
            e.LastLogin
        )).ToList();

        return new PagedResult<EmployeeListItem>(items, totalCount, request.PageNumber, request.PageSize);
    }
}
