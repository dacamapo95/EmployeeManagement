using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Shared.Results;

namespace EmployeeManagement.Application.Features.Employees.Queries.GetStatuses;

public sealed class GetEmployeeStatusesQueryHandler(IEmployeeStatusRepository employeeStatusRepository)
    : IQueryHandler<GetEmployeeStatusesQuery, IReadOnlyList<EmployeeStatusResponse>>
{
    private readonly IEmployeeStatusRepository _employeeStatusRepository = employeeStatusRepository;

    public async Task<Result<IReadOnlyList<EmployeeStatusResponse>>> Handle(GetEmployeeStatusesQuery request, CancellationToken cancellationToken)
    {
        var statuses = await _employeeStatusRepository.GetAllAsync(cancellationToken);

        var response = statuses
            .Select(s => new EmployeeStatusResponse(s.Id, s.Name))
            .OrderBy(s => s.Name)
            .ToList();

        return response;
    }
}
