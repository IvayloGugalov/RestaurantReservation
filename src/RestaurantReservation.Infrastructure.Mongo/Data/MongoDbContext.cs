using Microsoft.Extensions.Options;

namespace RestaurantReservation.Infrastructure.Mongo.Data;

public class MongoDbContext : IMongoDbContext
{
    public IClientSessionHandle? Session { get; set; }
    public IMongoDatabase Database { get; }
    public IMongoClient MongoClient { get; }
    protected readonly IList<Func<Task>> commands;

    protected MongoDbContext(IOptions<MongoOptions> options)
    {
        this.MongoClient = new MongoClient(options.Value.ConnectionString);
        var databaseName = options.Value.DatabaseName;
        this.Database = this.MongoClient.GetDatabase(databaseName);

        // Every command will be stored and it'll be processed at SaveChanges
        this.commands = new List<Func<Task>>();
    }

    public IMongoCollection<T> GetCollection<T>(string? name = null)
    {
        return this.Database.GetCollection<T>(name ?? typeof(T).Name.ToLower());
    }

    public void Dispose()
    {
        while (this.Session is { IsInTransaction: true })
            Thread.Sleep(TimeSpan.FromMilliseconds(100));

        GC.SuppressFinalize(this);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var result = this.commands.Count;

        using (this.Session = await this.MongoClient.StartSessionAsync(cancellationToken: ct))
        {
            this.Session.StartTransaction();

            try
            {
                var commandTasks = this.commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await this.Session.CommitTransactionAsync(ct);
            }
            catch (Exception ex)
            {
                await this.Session.AbortTransactionAsync(ct);
                this.commands.Clear();
                throw;
            }
        }

        this.commands.Clear();
        return result;
    }

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        this.Session = await this.MongoClient.StartSessionAsync(cancellationToken: ct);
        this.Session.StartTransaction();
    }

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        if (this.Session is { IsInTransaction: true })
            await this.Session.CommitTransactionAsync(ct);

        this.Session?.Dispose();
    }

    public async Task RollbackTransaction(CancellationToken ct = default)
    {
        await this.Session?.AbortTransactionAsync(ct)!;
    }

    public void AddCommand(Func<Task> func)
    {
        this.commands.Add(func);
    }

    public async Task ExecuteTransactionalAsync(Func<Task> action, CancellationToken ct = default)
    {
        await BeginTransactionAsync(ct);
        try
        {
            await action();

            await CommitTransactionAsync(ct);
        }
        catch
        {
            await RollbackTransaction(ct);
            throw;
        }
    }

    public async Task<T> ExecuteTransactionalAsync<T>(
        Func<Task<T>> action,
        CancellationToken ct = default)
    {
        await BeginTransactionAsync(ct);
        try
        {
            var result = await action();

            await CommitTransactionAsync(ct);

            return result;
        }
        catch
        {
            await RollbackTransaction(ct);
            throw;
        }
    }
}
