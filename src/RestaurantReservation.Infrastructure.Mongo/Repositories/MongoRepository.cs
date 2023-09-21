using System.Linq.Expressions;

namespace RestaurantReservation.Infrastructure.Mongo.Repositories;

public class MongoRepository<TEntity, TId, TValue> : IMongoRepository<TEntity, TId, TValue>
    where TEntity : class, IEntity<TId>
    where TId : StronglyTypedId<TValue>
    where TValue : IEquatable<TValue>
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
        var result = await this.DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id.Value), cancellationToken: ct);
        return result.SingleOrDefault(cancellationToken: ct);
    }

    public async Task<TEntity?> FindOneAsync(FilterDefinition<TEntity> filter, CancellationToken ct = default)
    {
        var result = await this.DbSet.FindAsync(filter, cancellationToken: ct);
        return result.SingleOrDefault(ct);
    }

    public async Task<List<TEntity>> ListAsync(CancellationToken ct = default)
    {
        return await this.DbSet.AsQueryable().ToListAsync(ct);
    }

    public void AddAsync(TEntity entity, CancellationToken ct = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        this.context.AddCommand(() => this.DbSet.InsertOneAsync(entity, new InsertOneOptions(), ct));
    }

    public void UpdateAsync(TEntity entity, CancellationToken ct = default)
    {
        entity.LastModified = DateTime.UtcNow;
        this.context.AddCommand(() =>
            this.DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id), entity, new ReplaceOptions(), ct));
    }

    public void DeleteByIdAsync(TId id, CancellationToken ct = default)
    {
        this.context.AddCommand(() => this.DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id), ct));
    }

    public void DeleteRangeAsync(FilterDefinition<TEntity> filter, CancellationToken ct = default)
    {
        this.context.AddCommand(() => this.DbSet.DeleteManyAsync(filter, ct));
    }

    public bool Exists(Expression<Func<TEntity, object>> criteria, bool exists)
    {
        var builder = Builders<TEntity>.Filter;
        var filter = builder.Exists(criteria, exists);
        return this.DbSet.Find(filter).Any();
    }
}
