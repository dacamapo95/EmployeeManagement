using MediatR;
using EmployeeManagement.Shared.Results;

namespace EmployeeManagement.Application.Core.Abstractions;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;