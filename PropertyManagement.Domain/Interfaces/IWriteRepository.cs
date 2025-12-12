using EmployeeManagement.Shared.Primitives;

namespace EmployeeManagement.Domain.Interfaces;


public interface IWriteRepository<TEntity, TId> where TEntity : Entity<TId> where TId : IEquatable<TId>
{
    void Add(TEntity entity);

    void Update(TEntity entity);

    void Remove(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    void RemoveRange(IEnumerable<TEntity> entities);
}