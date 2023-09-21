using System.Linq.Expressions;

namespace RestaurantReservation.Infrastructure.Mongo.Repositories;

public interface IMongoRepository<TEntity, in TId, TValue> : IDisposable
    where TEntity : class, IEntity<TId>
    where TId : StronglyTypedId<TValue>
    where TValue : IEquatable<TValue>
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct = default);

    Task<TEntity?> FindOneAsync(FilterDefinition<TEntity> filter, CancellationToken ct = default);

    Task<List<TEntity>> ListAsync(CancellationToken ct = default);

    void AddAsync(TEntity entity, CancellationToken ct = default);

    void UpdateAsync(TEntity entity, CancellationToken ct = default);

    void DeleteByIdAsync(TId id, CancellationToken ct = default);

    void DeleteRangeAsync(FilterDefinition<TEntity> filter, CancellationToken ct = default);

    bool Exists(Expression<Func<TEntity, object>> criteria, bool exists);
}
