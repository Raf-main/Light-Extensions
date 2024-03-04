using Light.Core.Extensions.Entities.Interfaces;
using Light.Infrastructure.Extensions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Light.Infrastructure.EfCore.Repositories;

public class GenericEfRepository<TEntity, TKey> : IAsyncGenericRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey> where TKey : struct
{
    protected readonly DbContext Context;
    protected readonly IAsyncReadRepository<TEntity, TKey> ReadRepository;
    protected readonly DbSet<TEntity> Table;
    protected readonly IAsyncWriteRepository<TEntity, TKey> WriteRepository;

    public GenericEfRepository(DbContext context)
    {
        Context = context;
        Table = context.Set<TEntity>();
        ReadRepository = new ReadEfRepository<TEntity, TKey>(context);
        WriteRepository = new WriteEfRepository<TEntity, TKey>(context);
    }

    public GenericEfRepository(DbContext context,
        IAsyncReadRepository<TEntity, TKey> readRepository,
        IAsyncWriteRepository<TEntity, TKey> writeRepository
    )
    {
        Context = context;
        ReadRepository = readRepository;
        WriteRepository = writeRepository;
        Table = context.Set<TEntity>();
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await WriteRepository.AddAsync(entity, cancellationToken);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default
    )
    {
        await WriteRepository.AddRangeAsync(entities, cancellationToken);
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await WriteRepository.UpdateAsync(entity, cancellationToken);
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await WriteRepository.DeleteAsync(entity, cancellationToken);
    }

    public virtual async Task<TEntity?> GetByKeyAsync(TKey key, CancellationToken cancellationToken = default)
    {
        return await ReadRepository.GetByKeyAsync(key, cancellationToken);
    }

    public IQueryable<TEntity> Find()
    {
        return ReadRepository.Find();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await ReadRepository.GetAllAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> GetPagedAsync(int skip,
        int take,
        CancellationToken cancellationToken = default
    )
    {
        return await ReadRepository.GetPagedAsync(skip, take, cancellationToken);
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await ReadRepository.CountAsync(cancellationToken);
    }
}
