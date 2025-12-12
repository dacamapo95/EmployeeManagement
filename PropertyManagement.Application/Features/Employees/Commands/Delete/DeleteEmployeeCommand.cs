using EmployeeManagement.Application.Core.Abstractions;

namespace EmployeeManagement.Application.Features.Employees.Commands.Delete;

public sealed record DeleteEmployeeCommand(Guid Id) : ICommand;
