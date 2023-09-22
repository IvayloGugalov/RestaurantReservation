using RestaurantReservation.Core.Model;
using RestaurantReservation.Core.Repository;

namespace RestaurantReservation.Core.Mongo;

public interface IMongoRepository<TEntity, in TId> : IRepositoryBase<TEntity, TId>, IDisposable
    where TEntity : class, IEntity<TId>
    where TId : IEquatable<TId>
{

}
