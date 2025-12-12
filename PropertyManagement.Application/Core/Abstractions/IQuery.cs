using MediatR;
using EmployeeManagement.Shared.Results;

namespace EmployeeManagement.Application.Core.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;