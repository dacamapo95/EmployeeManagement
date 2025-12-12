using Carter;
using System.Security.Claims;

namespace EmployeeManagement.API.Endpoints;

public sealed class Authentication : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth").WithOpenApi();

        group.MapGet("/info", (IConfiguration configuration) =>
        {
            var authority = configuration["Keycloak:Authority"];
            var tokenEndpoint = $"{authority}/protocol/openid-connect/token";
            var authEndpoint = $"{authority}/protocol/openid-connect/auth";

            return Results.Ok(new
            {
                message = "Esta API usa Keycloak para autenticacion OAuth2/OpenID Connect",
                keycloakAdmin = new
                {
                    url = "http://localhost:8080",
                    username = "admin",
                    password = "admin",
                    realm = "employeemanagement"
                },
                testUsers = new[]
                {
                    new { username = "testuser", password = "test123", roles = new[] { "employee" } },
                    new { username = "manager", password = "manager123", roles = new[] { "manager", "employee" } },
                    new { username = "admin", password = "admin123", roles = new[] { "admin", "manager", "employee" } }
                },
                endpoints = new
                {
                    tokenEndpoint,
                    authEndpoint,
                    userInfoEndpoint = $"{authority}/protocol/openid-connect/userinfo",
                    logoutEndpoint = $"{authority}/protocol/openid-connect/logout"
                },
                postmanExample = new
                {
                    method = "POST",
                    url = tokenEndpoint,
                    body = new
                    {
                        grant_type = "password",
                        client_id = "postman-client",
                        username = "testuser",
                        password = "test123"
                    },
                    headers = new
                    {
                        ContentType = "application/x-www-form-urlencoded"
                    },
                    note = "En Postman, selecciona 'x-www-form-urlencoded' y agrega los campos como key-value pairs"
                },
                curlExample = $"curl -X POST '{tokenEndpoint}' -H 'Content-Type: application/x-www-form-urlencoded' -d 'grant_type=password&client_id=postman-client&username=testuser&password=test123'"
            });
        })
        .WithName("GetAuthInfo")
        .WithSummary("Obtener informacion de autenticacion")
        .WithDescription("Retorna informacion sobre como autenticarse con Keycloak, incluyendo usuarios de prueba y ejemplos.")
        .Produces(StatusCodes.Status200OK)
        .AllowAnonymous();
    }
}
