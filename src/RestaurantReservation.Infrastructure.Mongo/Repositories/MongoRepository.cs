using System.Linq.Expressions;

namespace RestaurantReservation.Infrastructure.Mongo.Repositories;

public class MongoRepository<TEntity, TId> : IMongoRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>
    where TId : IEquatable<TId>
{
    private readonly IMongoDbContext context;
    protected readonly IMongoCollection<TEntity> DbSet;

    public MongoRepository(IMongoDbContext context)
    {
        this.context = context;
        this.DbSet = this.context.GetCollection<TEntity>();
    }

    public void Dispose()
    {
        this.context?.Dispose();
    }

    public Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return this.FindOneAsync(e => e.Id.Equals(id), cancellationToken);
    }

    public Task<TEntity?> FindOneAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return DbSet.Find(predicate).SingleOrDefaultAsync(cancellationToken: cancellationToken)!;
    }

    public async Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await this.DbSet.AsQueryable().ToListAsync(cancellationToken);
    }

    public Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
    {
        return this.DbSet.Find(filter).SingleOrDefaultAsync(cancellationToken: cancellationToken)!;
    }

    public Task<TEntity> SingleOrDefaultAsync(StronglyTypedId<TId> id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq("_id", id);
        return this.DbSet.Find(filter).SingleOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await this.DbSet.InsertOneAsync(entity, new InsertOneOptions(), cancellationToken);

        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await this.DbSet.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity, new ReplaceOptions(), cancellationToken);

        return entity;
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return this.DbSet.DeleteOneAsync(e => e.Id.Equals(entity.Id), cancellationToken);
    }

    public Task DeleteByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return this.DbSet.DeleteOneAsync(e => e.Id.Equals(id), cancellationToken);
    }

    public Task DeleteRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        return this.DbSet.DeleteOneAsync(e => entities.Any(i => e.Id.Equals(i.Id)), cancellationToken);
    }

    public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return this.DbSet.DeleteOneAsync(predicate, cancellationToken);
    }
}
