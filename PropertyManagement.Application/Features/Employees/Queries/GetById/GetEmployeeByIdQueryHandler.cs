using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Shared.Results;

namespace EmployeeManagement.Application.Features.Employees.Queries.GetById;

public sealed class GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository)
    : IQueryHandler<GetEmployeeByIdQuery, EmployeeResponse>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;

    public async Task<Result<EmployeeResponse>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetDetailsByIdAsync(request.Id, cancellationToken);

        if (employee is null)
            return EmployeeErrors.NotFound(request.Id);

        var response = new EmployeeResponse(
            employee.Id,
            employee.CompanyId,
            employee.Company.Name,
            employee.PortalId,
            employee.Portal.Name,
            employee.RoleId,
            employee.Role.Name,
            employee.StatusId,
            employee.Status.Name,
            employee.Username,
            employee.Email,
            employee.Name,
            employee.Telephone,
            employee.Fax,
            employee.LastLogin,
            employee.CreatedAtUtc,
            employee.LastModifiedAtUtc
        );

        return response;
    }
}
