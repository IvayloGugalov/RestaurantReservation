using RestaurantReservation.Core.Repository;

namespace RestaurantReservation.Infrastructure.Mongo.Repositories;

public interface IMongoRepository<TEntity, TId> : IRepositoryBase<TEntity, TId>
    where TEntity : class, IEntity<TId>
    where TId : IEquatable<TId>
{
}
