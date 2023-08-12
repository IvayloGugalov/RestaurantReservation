using System.Linq.Expressions;
using RestaurantReservation.Core.Model;

namespace RestaurantReservation.Core.Repository;

public class RepositoryBase<T, TId> : IRepositoryBase<T, TId>
    where T : IEntity<TId>
    where TId : IEquatable<TId>
{
    private List<T> Entities = new List<T>();

    public async Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return Entities.Find(x => x.Id.Equals(id));
    }

    public async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return Entities;
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
    {
        return Entities.FirstOrDefault(filter.Compile());
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        Entities.Add(entity);
        return entity;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        Entities.AddRange(entities);
        return entities;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var toRemove = Entities.Find(x => x.Id.Equals(entity.Id));
        if (toRemove == null) Entities.Add(entity);
        else
        {
            Entities.Remove(toRemove);
            Entities.Add(entity);
        }
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        Entities.Remove(entity);
    }

    public async Task<T> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
