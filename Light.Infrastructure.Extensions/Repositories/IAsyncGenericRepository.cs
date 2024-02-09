using Light.Core.Extensions.Entities.Interfaces;

namespace Light.Infrastructure.Extensions.Repositories;

public interface IAsyncGenericRepository<TEntity, TKey> : IAsyncWriteRepository<TEntity, TKey>,
    IAsyncReadRepository<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct;