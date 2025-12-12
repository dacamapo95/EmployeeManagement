using EmployeeManagement.Infrastructure;
using EmployeeManagement.Application;
using Serilog;
namespace EmployeeManagement.API.Extensions;

public static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, loggerConfig) =>
            loggerConfig.ReadFrom.Configuration(context.Configuration));

        builder.Services.RegisterApiServices(builder.Configuration)
            .AddInfrastructureServices(builder.Configuration)
            .AddApplicationServices();

        return builder.Build();
    }
}
