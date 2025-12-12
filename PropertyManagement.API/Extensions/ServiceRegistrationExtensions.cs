using Carter;
using EmployeeManagement.API.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace EmployeeManagement.API.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer()
                .AddSwaggerGen(options =>
                {
                    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Description = "Enter 'Bearer' followed by a space and your JWT token."
                    });

                    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                    {
                        {
                            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                            {
                                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                                {
                                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                })
                .AddProblemDetails()
                .AddExceptionHandler<GlobalExceptionHandler>()
                .AddCarter()
                .AddMemoryCache()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
               

        services.AddAuthorization();

        return services;
    }
}