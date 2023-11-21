using Light.Infrastructure.MongoDb.Contexts.Interfaces;
using Light.Infrastructure.MongoDb.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Light.Infrastructure.MongoDb.Contexts;

public abstract class MongoDbContext : IMongoDbContext
{
    protected readonly IList<Func<Task>> Commands;

    protected MongoDbContext(MongoOptions options)
    {
        RegisterConventions();
        MongoClient = new MongoClient(options.ConnectionString);
        var databaseName = options.DatabaseName;
        Database = MongoClient.GetDatabase(databaseName);
        Commands = new List<Func<Task>>();
    }

    public IClientSessionHandle? Session { get; set; }
    public IMongoDatabase Database { get; }
    public IMongoClient MongoClient { get; }

    public IMongoCollection<T> GetCollection<T>(string? name = null)
    {
        return Database.GetCollection<T>(name ?? typeof(T).Name.ToLower());
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = Commands.Count;

        using (Session = await MongoClient.StartSessionAsync(cancellationToken: cancellationToken))
        {
            Session.StartTransaction();

            try
            {
                var commandTasks = Commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await Session.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception)
            {
                await Session.AbortTransactionAsync(cancellationToken);
                Commands.Clear();

                throw;
            }
        }

        Commands.Clear();

        return result;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        Session = await MongoClient.StartSessionAsync(cancellationToken: cancellationToken);
        Session.StartTransaction();
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Session is { IsInTransaction: true })
        {
            await Session.CommitTransactionAsync(cancellationToken);
        }

        Session?.Dispose();
    }

    public async Task RollbackTransaction(CancellationToken cancellationToken = default)
    {
        await Session?.AbortTransactionAsync(cancellationToken)!;
    }

    public void AddCommand(Func<Task> func)
    {
        Commands.Add(func);
    }

    private static void RegisterConventions()
    {
        ConventionRegistry.Register("conventions",
            new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
                new IgnoreIfDefaultConvention(false)
            }, _ => true);
    }

    public async Task ExecuteTransactionalAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        await BeginTransactionAsync(cancellationToken);

        try
        {
            await action();

            await CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransaction(cancellationToken);

            throw;
        }
    }

    public async Task<T> ExecuteTransactionalAsync<T>(Func<Task<T>> action,
        CancellationToken cancellationToken = default
    )
    {
        await BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await action();

            await CommitTransactionAsync(cancellationToken);

            return result;
        }
        catch
        {
            await RollbackTransaction(cancellationToken);

            throw;
        }
    }
}