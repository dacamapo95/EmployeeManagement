using Dapper;
using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Domain.Companies;
using EmployeeManagement.Domain.Employees;
using System.Text;

namespace EmployeeManagement.Infrastructure.Database.Repositories;

public sealed class EmployeeRepository(
    IDbConnectionFactory connectionFactory,
    ApplicationDbContext context)
    : Repository<Employee, Guid>(connectionFactory, context), IEmployeeRepository
{
    protected override string TableName => "Employees";

    public async Task<Employee?> GetDetailsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            SELECT e.*,
                   c.Id, c.Name,
                   p.Id, p.CompanyId, p.Name,
                   r.Id, r.Name,
                   s.Id, s.Name
            FROM EMY.Employees e
            INNER JOIN EMY.Companies c ON e.CompanyId = c.Id
            INNER JOIN EMY.Portals p ON e.PortalId = p.Id
            INNER JOIN EMY.Roles r ON e.RoleId = r.Id
            INNER JOIN EMY.EmployeeStatuses s ON e.StatusId = s.Id
            WHERE e.Id = @Id";

        var employeeDictionary = new Dictionary<Guid, Employee>();

        var result = await connection.QueryAsync<Employee, Company, Portal, Role, EmployeeStatus, Employee>(
            sql,
            (employee, company, portal, role, status) =>
            {
                if (!employeeDictionary.TryGetValue(employee.Id, out var employeeEntry))
                {
                    employeeEntry = employee;
                    employeeDictionary.Add(employee.Id, employeeEntry);
                }

                employeeEntry.Company = company;
                employeeEntry.Portal = portal;
                employeeEntry.Role = role;
                employeeEntry.Status = status;

                return employeeEntry;
            },
            new { Id = id },
            splitOn: "Id,Id,Id,Id");

        return result.FirstOrDefault();
    }

    public async Task<Employee?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM EMY.Employees WHERE Username = @Username";
        return await connection.QueryFirstOrDefaultAsync<Employee>(sql, new { Username = username });
    }

    public async Task<Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM EMY.Employees WHERE Email = @Email";
        return await connection.QueryFirstOrDefaultAsync<Employee>(sql, new { Email = email });
    }

    public async Task<bool> IsUsernameUniqueAsync(string username, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = "SELECT COUNT(*) FROM EMY.Employees WHERE Username = @Username";
        if (excludeId.HasValue)
            sql += " AND Id != @ExcludeId";

        var count = await connection.ExecuteScalarAsync<int>(sql, new { Username = username, ExcludeId = excludeId });
        return count == 0;
    }

    public async Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = "SELECT COUNT(*) FROM EMY.Employees WHERE Email = @Email";
        if (excludeId.HasValue)
            sql += " AND Id != @ExcludeId";

        var count = await connection.ExecuteScalarAsync<int>(sql, new { Email = email, ExcludeId = excludeId });
        return count == 0;
    }

    public async Task<(IReadOnlyList<Employee> Items, int TotalCount)> SearchAsync(
        int pageNumber,
        int pageSize,
        string? search,
        int? companyId,
        int? portalId,
        int? roleId,
        int? statusId,
        string? orderBy,
        bool desc,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var whereConditions = new List<string> { "e.DeletedOn IS NULL" };
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(search))
        {
            whereConditions.Add("(LOWER(e.Name) LIKE @Search OR LOWER(e.Email) LIKE @Search OR LOWER(e.Username) LIKE @Search)");
            parameters.Add("Search", $"%{search.ToLower()}%");
        }

        if (companyId.HasValue)
        {
            whereConditions.Add("e.CompanyId = @CompanyId");
            parameters.Add("CompanyId", companyId.Value);
        }

        if (portalId.HasValue)
        {
            whereConditions.Add("e.PortalId = @PortalId");
            parameters.Add("PortalId", portalId.Value);
        }

        if (roleId.HasValue)
        {
            whereConditions.Add("e.RoleId = @RoleId");
            parameters.Add("RoleId", roleId.Value);
        }

        if (statusId.HasValue)
        {
            whereConditions.Add("e.StatusId = @StatusId");
            parameters.Add("StatusId", statusId.Value);
        }

        var whereClause = string.Join(" AND ", whereConditions);

        var countSql = $@"
            SELECT COUNT(*)
            FROM EMY.Employees e
            WHERE {whereClause}";

        var totalCount = await connection.ExecuteScalarAsync<int>(countSql, parameters);

        var orderByClause = orderBy?.ToLower() switch
        {
            "name" => desc ? "e.Name DESC" : "e.Name ASC",
            "email" => desc ? "e.Email DESC" : "e.Email ASC",
            "username" => desc ? "e.Username DESC" : "e.Username ASC",
            "company" => desc ? "c.Name DESC" : "c.Name ASC",
            "portal" => desc ? "p.Name DESC" : "p.Name ASC",
            "role" => desc ? "r.Name DESC" : "r.Name ASC",
            "status" => desc ? "s.Name DESC" : "s.Name ASC",
            "createdat" => desc ? "e.CreatedAtUtc DESC" : "e.CreatedAtUtc ASC",
            _ => desc ? "e.CreatedAtUtc DESC" : "e.CreatedAtUtc ASC"
        };

        parameters.Add("Offset", (pageNumber - 1) * pageSize);
        parameters.Add("PageSize", pageSize);

        var dataSql = $@"
            SELECT e.*,
                   c.Id, c.Name,
                   p.Id, p.CompanyId, p.Name,
                   r.Id, r.Name,
                   s.Id, s.Name
            FROM EMY.Employees e
            INNER JOIN EMY.Companies c ON e.CompanyId = c.Id
            INNER JOIN EMY.Portals p ON e.PortalId = p.Id
            INNER JOIN EMY.Roles r ON e.RoleId = r.Id
            INNER JOIN EMY.EmployeeStatuses s ON e.StatusId = s.Id
            WHERE {whereClause}
            ORDER BY {orderByClause}
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY";

        var employeeDictionary = new Dictionary<Guid, Employee>();

        var items = await connection.QueryAsync<Employee, Company, Portal, Role, EmployeeStatus, Employee>(
            dataSql,
            (employee, company, portal, role, status) =>
            {
                if (!employeeDictionary.TryGetValue(employee.Id, out var employeeEntry))
                {
                    employeeEntry = employee;
                    employeeDictionary.Add(employee.Id, employeeEntry);
                }

                employeeEntry.Company = company;
                employeeEntry.Portal = portal;
                employeeEntry.Role = role;
                employeeEntry.Status = status;

                return employeeEntry;
            },
            parameters,
            splitOn: "Id,Id,Id,Id");

        return (employeeDictionary.Values.ToList(), totalCount);
    }
}
