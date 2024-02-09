using Light.Core.Extensions.Entities.Interfaces;
using Light.Infrastructure.Extensions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Light.Infrastructure.EfCore.Repositories;

public class ReadEfRepository<TEntity, TKey>(DbContext context) : IAsyncReadRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey> where TKey : struct
{
    protected readonly DbContext Context = context;
    protected readonly DbSet<TEntity> Table = context.Set<TEntity>();

    protected virtual IQueryable<TEntity> AsQueryable => Table.AsQueryable();

    public virtual async Task<TEntity?> GetByKeyAsync(TKey key, CancellationToken cancellationToken = default)
    {
        return await AsQueryable.FirstOrDefaultAsync(e => e.Id.Equals(key), cancellationToken);
    }

    public IQueryable<TEntity> Find()
    {
        return Table.AsQueryable();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await AsQueryable.AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> GetPagedAsync(int skip,
        int take,
        CancellationToken cancellationToken = default
    )
    {
        return await AsQueryable.AsNoTracking().Skip(skip).Take(take).ToListAsync(cancellationToken);
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await AsQueryable.AsNoTracking().CountAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync(CancellationToken cancellationToken = default)
    {
        return await AsQueryable.FirstOrDefaultAsync(cancellationToken);
    }
}