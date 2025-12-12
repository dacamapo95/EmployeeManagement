using Carter;
using EmployeeManagement.API.Extensions;
using EmployeeManagement.Application.Features.Companies.Queries.GetCompanies;
using EmployeeManagement.Application.Features.Companies.Queries.GetPortals;
using MediatR;

namespace EmployeeManagement.API.Endpoints;

public sealed class Companies : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/companies").WithTags("Companies").WithOpenApi();

        group.MapGet("/", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetCompaniesQuery();
            var result = await sender.Send(query, cancellationToken);

            return result.IsValid
                ? Results.Ok(result.Value)
                : ResultExtension.ResultToResponse(result);
        })
        .WithName("GetCompanies")
        .WithSummary("Obtener todas las compañías")
        .WithDescription("Retorna la lista completa de compañías registradas en el sistema")
        .Produces<IReadOnlyList<CompanyResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet("/{companyId}/portals", async (
            int companyId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetPortalsByCompanyIdQuery(companyId);
            var result = await sender.Send(query, cancellationToken);

            return result.IsValid
                ? Results.Ok(result.Value)
                : ResultExtension.ResultToResponse(result);
        })
        .WithName("GetPortalsByCompany")
        .WithSummary("Obtener portales de una compañía")
        .WithDescription("Retorna la lista de portales que pertenecen a una compañía específica")
        .Produces<IReadOnlyList<PortalResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
