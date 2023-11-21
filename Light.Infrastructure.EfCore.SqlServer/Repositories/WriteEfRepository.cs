using Light.Core.Extensions.Entities.Interfaces;
using Light.Infrastructure.Extensions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Light.Infrastructure.EfCore.SqlServer.Repositories;

public class WriteEfRepository<TEntity, TKey> : IAsyncWriteRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey> where TKey : struct
{
    protected readonly DbContext Context;
    protected readonly DbSet<TEntity> Table;

    public WriteEfRepository(DbContext context)
    {
        Context = context;
        Table = context.Set<TEntity>();
    }

    protected virtual IQueryable<TEntity> AsQueryable => Table.AsQueryable();

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Table.AddAsync(entity, cancellationToken);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default
    )
    {
        await Table.AddRangeAsync(entities, cancellationToken);
    }

    public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Table.Update(entity);

        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Table.Remove(entity);

        return Task.CompletedTask;
    }
}