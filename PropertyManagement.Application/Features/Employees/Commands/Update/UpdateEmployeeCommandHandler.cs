using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Domain.Companies;
using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Shared.Results;

namespace EmployeeManagement.Application.Features.Employees.Commands.Update;

public sealed class UpdateEmployeeCommandHandler(
    IEmployeeRepository employeeRepository,
    ICompanyRepository companyRepository,
    IPortalRepository portalRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly IPortalRepository _portalRepository = portalRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (employee is null)
            return EmployeeErrors.NotFound(request.Id);

        var companyExists = await _companyRepository.ExistsAsync(request.CompanyId, cancellationToken);
        if (!companyExists)
            return EmployeeErrors.CompanyNotFound(request.CompanyId);

        var portal = await _portalRepository.GetByIdWithCompanyAsync(request.PortalId, cancellationToken);
        if (portal is null)
            return EmployeeErrors.PortalNotFound(request.PortalId);

        if (portal.CompanyId != request.CompanyId)
            return EmployeeErrors.PortalNotInCompany();

        var isEmailUnique = await _employeeRepository.IsEmailUniqueAsync(request.Email, request.Id, cancellationToken);
        if (!isEmailUnique)
            return EmployeeErrors.EmailAlreadyExists(request.Email);

        var updateResult = employee.UpdateProfile(request.Name, request.Email, request.Telephone, request.Fax);
        if (updateResult.IsFailure)
            return updateResult.Error;

        employee.CompanyId = request.CompanyId;
        employee.PortalId = request.PortalId;

        var roleResult = employee.ChangeRole((RoleEnum)request.RoleId);
        if (roleResult.IsFailure)
            return roleResult.Error;

        var statusResult = employee.ChangeStatus((EmployeeStatusEnum)request.StatusId);
        if (statusResult.IsFailure)
            return statusResult.Error;

        _employeeRepository.Update(employee);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
