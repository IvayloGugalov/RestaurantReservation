namespace RestaurantReservation.Infrastructure.Mongo.Data;

public interface ITransactionAble
{
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
}
