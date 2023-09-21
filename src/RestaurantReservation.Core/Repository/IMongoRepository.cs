using RestaurantReservation.Core.Model;

namespace RestaurantReservation.Core.Repository;

public interface IMongoRepository<TEntity, in TId> : IRepositoryBase<TEntity, TId>, IDisposable
    where TEntity : class, IEntity<TId>
    where TId : IEquatable<TId>
{

}
