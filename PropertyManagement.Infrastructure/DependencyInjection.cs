using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Infrastructure.Database;
using EmployeeManagement.Infrastructure.Database.Interceptors;
using EmployeeManagement.Infrastructure.Database.Interfaces;
using EmployeeManagement.Infrastructure.Database.Repositories;
using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Domain.Companies;

namespace EmployeeManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>())
                   .UseSqlServer(configuration.GetConnectionString("Default"), sql =>
                   {
                       sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                   });
        });

        services.AddHttpContextAccessor();
        services.AddScoped<ISaveChangesInterceptor, AuditableEntitySaveChangesInterceptor>();

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IPortalRepository, PortalRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IEmployeeStatusRepository, EmployeeStatusRepository>();

        return services;
    }
}
