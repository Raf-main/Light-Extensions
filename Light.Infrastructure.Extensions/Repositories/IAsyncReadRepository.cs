using Light.Core.Extensions.Entities.Interfaces;

namespace Light.Infrastructure.Extensions.Repositories;

public interface IAsyncReadRepository<TEntity, in TKey> where TEntity : IEntity<TKey> where TKey : struct
{
    IQueryable<TEntity> Find();

    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> GetPagedAsync(int skip, int take, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<TEntity?> GetByKeyAsync(TKey key, CancellationToken cancellationToken = default);
}