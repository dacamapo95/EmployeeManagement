using EmployeeManagement.Shared.Primitives;
namespace EmployeeManagement.Domain.Interfaces;

public interface IReadRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : IEquatable<TId>
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> GetAsync(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetFirstOrDefaultAsync(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        TId id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        string sql,
        object parameters,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> GetByIdsAsync(
        IEnumerable<TId> ids,
        CancellationToken cancellationToken = default);
}