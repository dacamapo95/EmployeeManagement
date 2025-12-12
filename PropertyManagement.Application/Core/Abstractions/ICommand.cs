using MediatR;
using EmployeeManagement.Shared.Results;

namespace EmployeeManagement.Application.Core.Abstractions;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
