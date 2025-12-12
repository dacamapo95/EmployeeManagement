using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Shared.Results;

namespace EmployeeManagement.Application.Features.Employees.Commands.Delete;

public sealed class DeleteEmployeeCommandHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);

        if (employee is null)
            return EmployeeErrors.NotFound(request.Id);

        _employeeRepository.Remove(employee);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
