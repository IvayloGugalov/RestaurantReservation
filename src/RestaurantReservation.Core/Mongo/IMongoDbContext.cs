using MongoDB.Driver;

namespace RestaurantReservation.Core.Mongo;

public interface IMongoDbContext : IDisposable
{
    IMongoCollection<T> GetCollection<T>(string? name = null);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransaction(CancellationToken ct = default);
    void AddCommand(Func<Task> func);
}
