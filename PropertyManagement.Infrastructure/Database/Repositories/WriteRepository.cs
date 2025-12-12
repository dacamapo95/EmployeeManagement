using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Shared.Primitives;

namespace EmployeeManagement.Infrastructure.Database.Repositories;

public class WriteRepository<TEntity, TId>(ApplicationDbContext context) : IWriteRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : IEquatable<TId>
{
    protected readonly ApplicationDbContext _context = context;

    public virtual void Add(TEntity entity)
    {
        _context.Add(entity);
    }

    public virtual void Update(TEntity entity)
    {
        _context.Update(entity);
    }

    public virtual void Remove(TEntity entity)
    {
        _context.Remove(entity);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _context.AddRangeAsync(entities, cancellationToken);
    }

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
    {
        _context.RemoveRange(entities);
    }
}
