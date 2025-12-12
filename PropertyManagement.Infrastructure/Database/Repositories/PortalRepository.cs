using Dapper;
using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Domain.Companies;

namespace EmployeeManagement.Infrastructure.Database.Repositories;

public sealed class PortalRepository(IDbConnectionFactory connectionFactory) : ReadRepository<Portal, int>(connectionFactory), IPortalRepository
{
    protected override string TableName => "Portals";

    public async Task<Portal?> GetByIdWithCompanyAsync(int id, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            SELECT p.*, c.Id, c.Name
            FROM EMY.Portals p
            INNER JOIN EMY.Companies c ON p.CompanyId = c.Id
            WHERE p.Id = @Id";

        var portalDictionary = new Dictionary<int, Portal>();

        var result = await connection.QueryAsync<Portal, Company, Portal>(
            sql,
            (portal, company) =>
            {
                if (!portalDictionary.TryGetValue(portal.Id, out var portalEntry))
                {
                    portalEntry = portal;
                    portalDictionary.Add(portal.Id, portalEntry);
                }

                portalEntry.Company = company;
                return portalEntry;
            },
            new { Id = id },
            splitOn: "Id");

        return result.FirstOrDefault();
    }

    public async Task<IReadOnlyList<Portal>> GetByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            SELECT Id, Name, CompanyId
            FROM EMY.Portals
            WHERE CompanyId = @CompanyId
            ORDER BY Name";

        var portals = await connection.QueryAsync<Portal>(sql, new { CompanyId = companyId });
        return portals.ToList();
    }
}
