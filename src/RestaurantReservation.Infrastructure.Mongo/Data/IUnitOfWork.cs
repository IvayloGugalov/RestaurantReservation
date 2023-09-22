using RestaurantReservation.Core.Mongo;

namespace RestaurantReservation.Infrastructure.Mongo.Data;

public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitAsync(CancellationToken ct = default);
}

public interface IUnitOfWork<out TContext> : IUnitOfWork
    where TContext : class
{
    TContext Context { get; }
}

public interface IMongoUnitOfWork<out TContext> : IUnitOfWork<TContext> where TContext : class, IMongoDbContext
{
}
