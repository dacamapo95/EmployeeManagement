using Carter;

namespace EmployeeManagement.API.Endpoints;

public sealed class Authentication : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth").WithOpenApi();

        group.MapPost("/login", () =>
        {
            return Results.Problem(
                statusCode: StatusCodes.Status501NotImplemented,
                title: "Not Implemented",
                detail: "Authentication will be implemented with external identity provider");
        })
        .WithName("Login")
        .WithSummary("Iniciar sesi�n en el sistema")
        .WithDescription("TEMPORALMENTE DESHABILITADO: La autenticaci�n ser� implementada con un proveedor de identidad externo.")
        .ProducesProblem(StatusCodes.Status501NotImplemented);

        group.MapPost("/refresh", () =>
        {
            return Results.Problem(
                statusCode: StatusCodes.Status501NotImplemented,
                title: "Not Implemented",
                detail: "Token refresh will be implemented with external identity provider");
        })
        .WithName("RefreshToken")
        .WithSummary("Renovar token de acceso")
        .WithDescription("TEMPORALMENTE DESHABILITADO: La renovaci�n de tokens ser� implementada con un proveedor de identidad externo.")
        .ProducesProblem(StatusCodes.Status501NotImplemented);
    }
}