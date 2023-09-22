using System.Linq.Expressions;
using RestaurantReservation.Core.Model;

namespace RestaurantReservation.Core.Repository;

public interface IRepositoryBase<TEntity, in TId>
    where TEntity : class, IEntity<TId>
    where TId : IEquatable<TId>
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct = default);

    Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> filter, CancellationToken ct = default);

    Task<List<TEntity>> ListAsync(CancellationToken ct = default);

    Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default);

    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken ct = default);

    Task DeleteByIdAsync(TId id, CancellationToken ct = default);

    Task DeleteRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken ct = default);

    bool Exists(Expression<Func<TEntity, object>> criteria, bool exists);
}
