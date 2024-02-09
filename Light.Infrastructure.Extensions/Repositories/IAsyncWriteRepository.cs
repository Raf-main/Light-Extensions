using Light.Core.Extensions.Entities.Interfaces;

namespace Light.Infrastructure.Extensions.Repositories;

public interface IAsyncWriteRepository<in TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}