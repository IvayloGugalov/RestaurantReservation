using System.Linq.Expressions;
using RestaurantReservation.Core.Model;

namespace RestaurantReservation.Core.Repository;

public interface IRepositoryBase<T, in TId>
    where T : class, IEntity<TId>
    where TId : IEquatable<TId>
{
    /// <summary>
    /// Finds an entity with the given primary key value.
    /// </summary>
    Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds all entities of <typeparamref name="T" /> from the database.
    /// </summary>
    Task<List<T>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
    /// </summary>
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds an entity in the database.
    /// </summary>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds the given entities in the database
    /// </summary>
    // Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an entity in the database
    /// </summary>
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes an entity in the database
    /// </summary>
    void DeleteAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists changes to the database.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
