using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Domain.Companies;
using EmployeeManagement.Shared.Results;

namespace EmployeeManagement.Application.Features.Companies.Queries.GetPortals;

public sealed class GetPortalsByCompanyIdQueryHandler(IPortalRepository portalRepository)
    : IQueryHandler<GetPortalsByCompanyIdQuery, IReadOnlyList<PortalResponse>>
{
    private readonly IPortalRepository _portalRepository = portalRepository;

    public async Task<Result<IReadOnlyList<PortalResponse>>> Handle(GetPortalsByCompanyIdQuery request, CancellationToken cancellationToken)
    {
        var portals = await _portalRepository.GetByCompanyIdAsync(request.CompanyId, cancellationToken);

        var response = portals
            .Select(p => new PortalResponse(p.Id, p.Name, p.CompanyId))
            .ToList();

        return response;
    }
}
