using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Domain.Companies;
using EmployeeManagement.Shared.Results;

namespace EmployeeManagement.Application.Features.Companies.Queries.GetCompanies;

public sealed class GetCompaniesQueryHandler(ICompanyRepository companyRepository)
    : IQueryHandler<GetCompaniesQuery, IReadOnlyList<CompanyResponse>>
{
    private readonly ICompanyRepository _companyRepository = companyRepository;

    public async Task<Result<IReadOnlyList<CompanyResponse>>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await _companyRepository.GetAllAsync(cancellationToken);

        var response = companies
            .Select(c => new CompanyResponse(c.Id, c.Name))
            .OrderBy(c => c.Name)
            .ToList();

        return response;
    }
}
