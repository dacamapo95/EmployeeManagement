using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EmployeeManagement.Infrastructure.Database.Interfaces;
using EmployeeManagement.Domain.Companies;

namespace EmployeeManagement.Infrastructure.Database;

public sealed class DatabaseInitializer(ApplicationDbContext db, ILogger<DatabaseInitializer> logger) : IDatabaseInitializer
{
    private readonly ApplicationDbContext _db = db;
    private readonly ILogger<DatabaseInitializer> _logger = logger;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await EnsureDatabaseCreatedAsync(cancellationToken);
        await SeedDataAsync(cancellationToken);
    }

    private async Task EnsureDatabaseCreatedAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Applying database migrations...");
            await _db.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Database migrations applied.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while applying database migrations.");
            throw;
        }
    }

    private async Task SeedDataAsync(CancellationToken cancellationToken)
    {
        try
        {
            await SeedCompaniesAsync(cancellationToken);
            await SeedPortalsAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding data.");
            throw;
        }
    }

    private async Task SeedCompaniesAsync(CancellationToken cancellationToken)
    {
        if (await _db.Set<Company>().AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Companies already seeded. Skipping...");
            return;
        }

        _logger.LogInformation("Seeding companies...");

        var companies = new List<Company>
        {
            new Company { Name = "Acme Corporation" },
            new Company { Name = "TechStart Inc" },
            new Company { Name = "Global Solutions Ltd" },
            new Company { Name = "Innovation Labs" },
            new Company { Name = "Digital Dynamics" }
        };

        await _db.Set<Company>().AddRangeAsync(companies, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Companies seeded successfully.");
    }

    private async Task SeedPortalsAsync(CancellationToken cancellationToken)
    {
        if (await _db.Set<Portal>().AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Portals already seeded. Skipping...");
            return;
        }

        _logger.LogInformation("Seeding portals...");

        var companies = await _db.Set<Company>().ToListAsync(cancellationToken);

        var portals = new List<Portal>
        {
            // Portals for Acme Corporation
            new Portal
            {
                Name = "Portal Principal",
                CompanyId = companies.First(c => c.Name == "Acme Corporation").Id,
            },
            new Portal
            {
                Name = "Portal Secundario",
                CompanyId = companies.First(c => c.Name == "Acme Corporation").Id,
            },

            // Portals for TechStart Inc
            new Portal
            {
                Name = "Portal Corporativo",
                CompanyId = companies.First(c => c.Name == "TechStart Inc").Id,
            },
            new Portal
            {
                Name = "Portal de Desarrolladores",
                CompanyId = companies.First(c => c.Name == "TechStart Inc").Id,
            },

            // Portals for Global Solutions Ltd
            new Portal
            {
                Name = "Portal Global",
                CompanyId = companies.First(c => c.Name == "Global Solutions Ltd").Id,
            },

            // Portals for Innovation Labs
            new Portal
            {
                Name = "Portal de Innovación",
                CompanyId = companies.First(c => c.Name == "Innovation Labs").Id,
            },
            new Portal
            {
                Name = "Portal de Investigación",
                CompanyId = companies.First(c => c.Name == "Innovation Labs").Id,
            },

            // Portals for Digital Dynamics
            new Portal
            {
                Name = "Portal Digital",
                CompanyId = companies.First(c => c.Name == "Digital Dynamics").Id,
            }
        };

        await _db.Set<Portal>().AddRangeAsync(portals, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Portals seeded successfully.");
    }
}
