using System.Linq.Expressions;
using RestaurantReservation.Core.Mongo;

namespace RestaurantReservation.Infrastructure.Mongo.Repositories;

public class MongoRepository<TEntity, TId> : IMongoRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>
    where TId : IEquatable<TId>
{
    private readonly IMongoDbContext context;
    private readonly IMongoCollection<TEntity> DbSet;

    public MongoRepository(IMongoDbContext context)
    {
        this.context = context;
        this.DbSet = this.context.GetCollection<TEntity>();
    }

    public void Dispose()
    {
        this.context.Dispose();
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct = default)
    {
        var result = await this.DbSet.FindAsync(x => x.Id.Equals(id), cancellationToken: ct);
        return result.SingleOrDefault(cancellationToken: ct);
    }

    public async Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> filter, CancellationToken ct = default)
    {
        var result = await this.DbSet.FindAsync(filter, cancellationToken: ct);
        return result.SingleOrDefault(ct);
    }

    public async Task<List<TEntity>> ListAsync(CancellationToken ct = default)
    {
        return await this.DbSet.AsQueryable().ToListAsync(ct);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await this.DbSet.InsertOneAsync(entity, new InsertOneOptions(), ct);
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken ct = default)
    {
        entity.LastModified = DateTime.UtcNow;
        await this.DbSet.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity, new ReplaceOptions(), ct);
        return entity;
    }

    public Task DeleteByIdAsync(TId id, CancellationToken ct = default)
    {
        return this.DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id), ct);
    }

    public Task DeleteRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken ct = default)
    {
        return this.DbSet.DeleteOneAsync(e => entities.Any(i => e.Id.Equals(i.Id)), ct);
    }

    public bool Exists(Expression<Func<TEntity, object>> criteria, bool exists)
    {
        var builder = Builders<TEntity>.Filter;
        var filter = builder.Exists(criteria, exists);
        return this.DbSet.Find(filter).Any();
    }
}
