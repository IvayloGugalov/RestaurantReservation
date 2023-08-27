using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RestaurantReservation.Core.Model;
using RestaurantReservation.Core.Repository;

namespace RestaurantReservation.Infrastructure.EF.Data.Repository;

public class RepositoryBase<T, TId> : IRepositoryBase<T, TId>
    where T : class, IEntity<TId>
    where TId : IEquatable<TId>
{
    private readonly IDbContext dbContext;

    public RepositoryBase(IDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return this.dbContext.Set<T, TId>().SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return this.dbContext.Set<T, TId>().ToListAsync(cancellationToken);
    }

    public Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken = default)
    {
        return this.dbContext.Set<T, TId>().SingleOrDefaultAsync(filter, cancellationToken);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        var result = await this.dbContext.Set<T, TId>().AddAsync(entity, cancellationToken);
        return result.Entity;
    }

    // public async Task<IEnumerable<T>> AddRangeAsync(params T[] entities, CancellationToken cancellationToken = default)
    // {
    //     var result = this.dbContext.Set<T, TId>().AddRange(entities, cancellationToken);
    //     return entities;
    // }

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var toUpdate = await this.dbContext.Set<T, TId>()
            .SingleOrDefaultAsync(x => x.Id.Equals(entity.Id), cancellationToken);
        EntityEntry<T> result;
        if (toUpdate == null) result = await this.dbContext.Set<T, TId>().AddAsync(entity, cancellationToken);
        else
        {
            result = this.dbContext.Set<T, TId>().Update(toUpdate);
        }

        return result.Entity;
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        this.dbContext.Set<T, TId>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return this.dbContext.SaveChangesAsync(cancellationToken);
    }
}
