using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using RestaurantReservation.Core.Model;

namespace RestaurantReservation.Infrastructure.EF.Data.Repository;

public class RepositoryBase<T, TId>
    where T : class, IEntity<TId>
    where TId : IEquatable<TId>
{
    private readonly IDbContext dbContext;

    public RepositoryBase(IDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task<T?> GetByIdAsync(TId id, CancellationToken ct = default) =>
        this.dbContext.Set<T, TId>().SingleOrDefaultAsync(x => x.Id.Equals(id), ct);

    public Task<List<T>> ListAsync(CancellationToken ct = default) =>
        this.dbContext.Set<T, TId>().ToListAsync(ct);

    public Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default) =>
        this.dbContext.Set<T, TId>().SingleOrDefaultAsync(filter, ct);

    public async Task<T> SingleOrDefaultAsync(TId id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        var result = await this.dbContext.Set<T, TId>().AddAsync(entity, ct);
        return result.Entity;
    }

    public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default) =>
        this.dbContext.Set<T, TId>().AddRangeAsync(entities, ct);

    public async Task<T> UpdateAsync(T entity, CancellationToken ct = default)
    {
        var toUpdate = await this.dbContext.Set<T, TId>()
            .SingleOrDefaultAsync(x => x.Id.Equals(entity.Id), ct);
        EntityEntry<T> result;
        if (toUpdate == null) result = await this.dbContext.Set<T, TId>().AddAsync(entity, ct);
        else
        {
            result = this.dbContext.Set<T, TId>().Update(toUpdate);
        }

        return result.Entity;
    }

    public Task DeleteAsync(T entity, CancellationToken ct = default) =>
        Task.FromResult(this.dbContext.Set<T, TId>().Remove(entity));

    public Task DeleteByIdAsync(TId id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRangeAsync(IReadOnlyList<T> entities, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        this.dbContext.SaveChangesAsync(ct);
}
