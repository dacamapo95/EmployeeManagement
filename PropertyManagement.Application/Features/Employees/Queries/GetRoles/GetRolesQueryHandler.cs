using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Shared.Results;

namespace EmployeeManagement.Application.Features.Employees.Queries.GetRoles;

public sealed class GetRolesQueryHandler(IRoleRepository roleRepository)
    : IQueryHandler<GetRolesQuery, IReadOnlyList<RoleResponse>>
{
    private readonly IRoleRepository _roleRepository = roleRepository;

    public async Task<Result<IReadOnlyList<RoleResponse>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllAsync(cancellationToken);

        var response = roles
            .Select(r => new RoleResponse(r.Id, r.Name))
            .OrderBy(r => r.Name)
            .ToList();

        return response;
    }
}
