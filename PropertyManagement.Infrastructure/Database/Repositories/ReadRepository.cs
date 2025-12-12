using Dapper;
using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Shared.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Infrastructure.Database.Repositories;


public class ReadRepository<TEntity, TId>(IDbConnectionFactory connectionFactory) : IReadRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : IEquatable<TId>
{
    protected readonly IDbConnectionFactory _connectionFactory = connectionFactory;

    protected virtual string SchemaName => "EMY";

    protected virtual string TableName
    {
        get
        {
            var tableAttribute = typeof(TEntity).GetCustomAttributes(typeof(TableAttribute), false)
                .FirstOrDefault() as TableAttribute;
            return tableAttribute?.Name ?? typeof(TEntity).Name + "s";
        }
    }

    protected virtual string FullTableName => $"{SchemaName}.{TableName}";

    protected virtual string IdColumnName => "Id";

    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = $"SELECT * FROM {FullTableName} WHERE {IdColumnName} = @Id";
        return await connection.QueryFirstOrDefaultAsync<TEntity>(sql, new { Id = id });
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = $"SELECT * FROM {FullTableName}";
        return await connection.QueryAsync<TEntity>(sql);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<TEntity>(sql, parameters);
    }

    public virtual async Task<TEntity?> GetFirstOrDefaultAsync(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<TEntity>(sql, parameters);
    }

    public virtual async Task<int> CountAsync(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, parameters);
    }

    public virtual async Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM {FullTableName} WHERE {IdColumnName} = @Id) THEN 1 ELSE 0 END";
        return await connection.ExecuteScalarAsync<bool>(sql, new { Id = id });
    }

    public virtual async Task<bool> ExistsAsync(
        string sql,
        object parameters,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<bool>(sql, parameters);
    }
    public virtual async Task<IEnumerable<TEntity>> GetByIdsAsync(
        IEnumerable<TId> ids,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = $"SELECT * FROM {FullTableName} WHERE {IdColumnName} IN @Ids";
        return await connection.QueryAsync<TEntity>(sql, new { Ids = ids });
    }
}
