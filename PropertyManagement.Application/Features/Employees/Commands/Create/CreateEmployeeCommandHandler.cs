using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Domain.Companies;
using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Shared.Results;

namespace EmployeeManagement.Application.Features.Employees.Commands.Create;

public sealed class CreateEmployeeCommandHandler(
    IEmployeeRepository employeeRepository,
    ICompanyRepository companyRepository,
    IPortalRepository portalRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateEmployeeCommand, CreateEmployeeResponse>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly IPortalRepository _portalRepository = portalRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<CreateEmployeeResponse>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var companyExists = await _companyRepository.ExistsAsync(request.CompanyId, cancellationToken);
        if (!companyExists)
            return EmployeeErrors.CompanyNotFound(request.CompanyId);

        var portal = await _portalRepository.GetByIdWithCompanyAsync(request.PortalId, cancellationToken);
        if (portal is null)
            return EmployeeErrors.PortalNotFound(request.PortalId);

        if (portal.CompanyId != request.CompanyId)
            return EmployeeErrors.PortalNotInCompany();

        var isUsernameUnique = await _employeeRepository.IsUsernameUniqueAsync(request.Username, null, cancellationToken);
        if (!isUsernameUnique)
            return EmployeeErrors.UsernameAlreadyExists(request.Username);

        var isEmailUnique = await _employeeRepository.IsEmailUniqueAsync(request.Email, null, cancellationToken);
        if (!isEmailUnique)
            return EmployeeErrors.EmailAlreadyExists(request.Email);

        var employee = new Employee
        {
            CompanyId = request.CompanyId,
            PortalId = request.PortalId,
            Username = request.Username,
            Email = request.Email,
            Password = request.Password,
            Name = request.Name,
            Telephone = request.Telephone,
            Fax = request.Fax
        };

        var roleResult = employee.ChangeRole((RoleEnum)request.RoleId);
        if (roleResult.IsFailure)
            return roleResult.Error;

        _employeeRepository.Add(employee);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateEmployeeResponse(employee.Id);
    }
}
