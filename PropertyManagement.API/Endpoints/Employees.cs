using Carter;
using EmployeeManagement.API.Extensions;
using EmployeeManagement.Application.Features.Employees.Commands.Create;
using EmployeeManagement.Application.Features.Employees.Commands.Delete;
using EmployeeManagement.Application.Features.Employees.Commands.Update;
using EmployeeManagement.Application.Features.Employees.Queries.Get;
using EmployeeManagement.Application.Features.Employees.Queries.GetById;
using EmployeeManagement.Application.Features.Employees.Queries.GetRoles;
using EmployeeManagement.Application.Features.Employees.Queries.GetStatuses;
using EmployeeManagement.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Endpoints;

public sealed class Employees : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/redarbor").WithTags("Employees").WithOpenApi();

        group.MapGet("/", async (
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] string? search,
            [FromQuery] int? companyId,
            [FromQuery] int? portalId,
            [FromQuery] int? roleId,
            [FromQuery] int? statusId,
            [FromQuery] string? orderBy,
            [FromQuery] bool desc,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetEmployeesQuery(
                pageNumber == 0 ? 1 : pageNumber,
                pageSize == 0 ? 20 : pageSize,
                search,
                companyId,
                portalId,
                roleId,
                statusId,
                orderBy,
                desc);

            var result = await sender.Send(query, cancellationToken);

            return result.IsValid
                ? Results.Ok(result.Value)
                : ResultExtension.ResultToResponse(result);
        })
        .WithName("GetEmployees")
        .WithSummary("Obtener lista de empleados")
        .WithDescription("Retorna una lista paginada de empleados con filtros opcionales")
        .Produces<PagedResult<EmployeeListItem>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .RequireAuthorization();

        group.MapGet("/{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetEmployeeByIdQuery(id);
            var result = await sender.Send(query, cancellationToken);

            return result.IsValid
                ? Results.Ok(result.Value)
                : ResultExtension.ResultToResponse(result);
        })
        .WithName("GetEmployeeById")
        .WithSummary("Obtener empleado por ID")
        .WithDescription("Retorna los detalles completos de un empleado espec�fico")
        .Produces<EmployeeResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .RequireAuthorization();

        group.MapPost("/", async (
            CreateEmployeeCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);

            return result.IsValid
                ? Results.CreatedAtRoute("GetEmployeeById", new { id = result.Value.Id }, result.Value)
                : ResultExtension.ResultToResponse(result);
        })
        .WithName("CreateEmployee")
        .WithSummary("Crear nuevo empleado")
        .WithDescription("Crea un nuevo empleado en el sistema. Requiere rol de Manager o Admin.")
        .Produces<CreateEmployeeResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .RequireAuthorization();

        group.MapPut("/{id:guid}", async (
            [FromRoute]Guid id,
            UpdateEmployeeCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
           
            var result = await sender.Send(command with { Id = id }, cancellationToken);

            return result.IsValid
                ? Results.NoContent()
                : ResultExtension.ResultToResponse(result);
        })
        .WithName("UpdateEmployee")
        .WithSummary("Actualizar empleado")
        .WithDescription("Actualiza la informaci�n de un empleado existente. Requiere rol de Manager o Admin.")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .RequireAuthorization();

        group.MapDelete("/{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteEmployeeCommand(id);
            var result = await sender.Send(command, cancellationToken);

            return result.IsValid
                ? Results.NoContent()
                : ResultExtension.ResultToResponse(result);
        })
        .WithName("DeleteEmployee")
        .WithSummary("Eliminar empleado")
        .WithDescription("Realiza una eliminaci�n l�gica (soft delete) de un empleado. Requiere rol de Admin.")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .RequireAuthorization();

        group.MapGet("/roles", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetRolesQuery();
            var result = await sender.Send(query, cancellationToken);

            return result.IsValid
                ? Results.Ok(result.Value)
                : ResultExtension.ResultToResponse(result);
        })
        .WithName("GetRoles")
        .WithSummary("Obtener roles de empleados")
        .WithDescription("Retorna la lista completa de roles disponibles para empleados")
        .Produces<IReadOnlyList<RoleResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .RequireAuthorization();

        group.MapGet("/statuses", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetEmployeeStatusesQuery();
            var result = await sender.Send(query, cancellationToken);

            return result.IsValid
                ? Results.Ok(result.Value)
                : ResultExtension.ResultToResponse(result);
        })
        .WithName("GetEmployeeStatuses")
        .WithSummary("Obtener estatus de empleados")
        .WithDescription("Retorna la lista completa de estatus disponibles para empleados")
        .Produces<IReadOnlyList<EmployeeStatusResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .RequireAuthorization();
    }
}
