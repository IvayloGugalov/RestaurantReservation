using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using RestaurantReservation.Core.Event;
using RestaurantReservation.Core.Model;

namespace RestaurantReservation.Core.EFCore;

public interface IDbContext
{
    DbSet<TEntity> Set<TEntity, TId>()
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>;

    IReadOnlyList<IDomainEvent> GetDomainEvents();
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);
    IExecutionStrategy CreateExecutionStrategy();
    Task ExecuteTransactionalAsync(CancellationToken ct = default);
}
