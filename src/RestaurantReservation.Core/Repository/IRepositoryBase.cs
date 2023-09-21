using System.Linq.Expressions;
using RestaurantReservation.Core.Model;

namespace RestaurantReservation.Core.Repository;

public interface IRepositoryBase<T, TId>
    where T : class, IEntity<TId>
    where TId : IEquatable<TId>
{
    /// <summary>
    /// Finds an entity with the given primary key value.
    /// </summary>
    Task<T?> GetByIdAsync(TId id, CancellationToken ct = default);

    /// <summary>
    /// Finds all entities of <typeparamref name="T" /> from the database.
    /// </summary>
    Task<List<T>> ListAsync(CancellationToken ct = default);

    /// <summary>
    /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
    /// </summary>
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default);

    // Task<T> SingleOrDefaultAsync(TId id, CancellationToken ct = default);

    /// <summary>
    /// Adds an entity in the database.
    /// </summary>
    Task<T> AddAsync(T entity, CancellationToken ct = default);

    /// <summary>
    /// Adds the given entities in the database
    /// </summary>
    // Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);

    /// <summary>
    /// Updates an entity in the database
    /// </summary>
    Task<T> UpdateAsync(T entity, CancellationToken ct = default);

    /// <summary>
    /// Removes an entity in the database
    /// </summary>
    Task DeleteAsync(T entity, CancellationToken ct = default);

    /// <summary>
    /// Removes an entity with the specified Id in the database
    /// </summary>
    /// <param name="id"></param>
    Task DeleteByIdAsync(TId id, CancellationToken ct = default);

    /// <summary>
    /// Removes a range of entities from the database
    /// </summary>
    /// <param name="entities"></param>
    Task DeleteRangeAsync(IReadOnlyList<T> entities, CancellationToken ct = default);

    /// <summary>
    /// Removes an entity based on the given expression
    /// </summary>
    /// <param name="predicate"></param>
    Task DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
}
