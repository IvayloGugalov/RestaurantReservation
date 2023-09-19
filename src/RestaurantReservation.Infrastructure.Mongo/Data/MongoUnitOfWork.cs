namespace RestaurantReservation.Infrastructure.Mongo.Data;

public class MongoUnitOfWork<TContext> : IMongoUnitOfWork<TContext>, ITransactionAble
    where TContext : MongoDbContext
{
    public MongoUnitOfWork(TContext context) => this.Context = context;

    public TContext Context { get; }

    public async Task CommitAsync(CancellationToken ct = default)
    {
        await this.Context.SaveChangesAsync(ct);
    }

    public Task BeginTransactionAsync(CancellationToken ct = default)
    {
        return this.Context.BeginTransactionAsync(ct);
    }

    public Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        return this.Context.RollbackTransaction(ct);
    }

    public Task CommitTransactionAsync(CancellationToken ct = default)
    {
        return this.Context.CommitTransactionAsync(ct);
    }

    public void Dispose() => this.Context.Dispose();
}
