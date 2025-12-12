using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Shared.Primitives;

namespace EmployeeManagement.Infrastructure.Database.Repositories;

public class Repository<TEntity, TId>(
    IDbConnectionFactory connectionFactory,
    ApplicationDbContext context)
    : ReadRepository<TEntity, TId>(connectionFactory), IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : IEquatable<TId>
{
    protected readonly ApplicationDbContext _context = context;
    private readonly WriteRepository<TEntity, TId> _writeRepository = new(context);

    public virtual void Add(TEntity entity) => _writeRepository.Add(entity);

    public virtual void Update(TEntity entity) => _writeRepository.Update(entity);

    public virtual void Remove(TEntity entity) => _writeRepository.Remove(entity);

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        => await _writeRepository.AddRangeAsync(entities, cancellationToken);

    public virtual void RemoveRange(IEnumerable<TEntity> entities) => _writeRepository.RemoveRange(entities);
}