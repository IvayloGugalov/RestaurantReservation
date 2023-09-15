namespace RestaurantReservation.Infrastructure.Mongo.Data;

public class MongoUnitOfWork<TContext> : IMongoUnitOfWork<TContext>, ITransactionAble
    where TContext : MongoDbContext
{
    public MongoUnitOfWork(TContext context) => Context = context;

    public TContext Context { get; }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await this.Context.SaveChangesAsync(cancellationToken);
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return this.Context.BeginTransactionAsync(cancellationToken);
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        return this.Context.RollbackTransaction(cancellationToken);
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        return this.Context.CommitTransactionAsync(cancellationToken);
    }

    public void Dispose() => this.Context.Dispose();
}
