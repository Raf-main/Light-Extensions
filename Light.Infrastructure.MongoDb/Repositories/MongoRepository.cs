using Light.Core.Extensions.Entities.Interfaces;
using Light.Infrastructure.Extensions.Repositories;
using Light.Infrastructure.MongoDb.Contexts.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Light.Infrastructure.MongoDb.Repositories;

public class MongoRepository<TEntity, TKey> : IAsyncGenericRepository<TEntity, TKey>
    where TEntity : IEntity<TKey> where TKey : struct
{
    protected readonly IMongoCollection<TEntity> Collection;
    protected readonly IMongoDbContext Context;

    public MongoRepository(IMongoDbContext context)
    {
        Context = context;
        Collection = Context.GetCollection<TEntity>();
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, null, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await Collection.InsertManyAsync(entities, null, cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.ReplaceOneAsync(e => e.Equals(entity.Id), entity, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.DeleteOneAsync(e => e.Equals(entity), cancellationToken);
    }

    public async Task<TEntity?> GetByKeyAsync(TKey key, CancellationToken cancellationToken = default)
    {
        return (await Collection.FindAsync(p => p.Id.Equals(key), cancellationToken: cancellationToken))
            .FirstOrDefault();
    }

    public IQueryable<TEntity> Find()
    {
        return Collection.AsQueryable();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Collection.Find(Builders<TEntity>.Filter.Empty).ToListAsync(cancellationToken);
    }

    public Task<IEnumerable<TEntity>> GetPagedAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}