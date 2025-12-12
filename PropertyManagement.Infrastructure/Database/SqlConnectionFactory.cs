using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using EmployeeManagement.Application.Core.Abstractions;

namespace EmployeeManagement.Infrastructure.Database;

public sealed class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not found.");
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
