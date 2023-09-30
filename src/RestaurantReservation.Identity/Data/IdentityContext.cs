using System.Collections.Immutable;
using System.Data;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using RestaurantReservation.Core.EFCore;
using RestaurantReservation.Core.Events;

namespace RestaurantReservation.Identity.Data;

public class IdentityContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>, IDbContext
{
    private readonly ILogger<IdentityContext>? logger;
    private IDbContextTransaction? currentTransaction;

    public IdentityContext(DbContextOptions<IdentityContext> options, ILogger<IdentityContext>? logger = null)
        : base(options)
    {
        this.logger = logger;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        OnBeforeSaving();
        try
        {
            return await base.SaveChangesAsync(ct);
        }
        //ref: https://learn.microsoft.com/en-us/ef/core/saving/concurrency?tabs=data-annotations#resolving-concurrency-conflicts
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                var databaseValues = await entry.GetDatabaseValuesAsync(ct);

                if (databaseValues == null)
                {
                    this.logger?.LogError("The record no longer exists in the database, The record has been deleted by another user.");
                    throw;
                }

                // Refresh the original values to bypass next concurrency check
                entry.OriginalValues.SetValues(databaseValues);
            }

            return await base.SaveChangesAsync(ct);
        }
    }

    public IExecutionStrategy CreateExecutionStrategy() => this.Database.CreateExecutionStrategy();

    public DbSet<TEntity> Set<TEntity, TId>() where TEntity : class, IEntity<TId> where TId : IEquatable<TId>
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        var domainEntities = ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.DomainEvents)
            .ToImmutableList();

        domainEntities.ForEach(entity => entity.ClearDomainEvents());

        return domainEvents;
    }

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        if (this.currentTransaction != null) return;

        this.currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        try
        {
            await SaveChangesAsync(ct);
            await this.currentTransaction?.CommitAsync(ct)!;
        }
        catch
        {
            await RollbackTransactionAsync(ct);
            throw;
        }
        finally
        {
            this.currentTransaction?.Dispose();
            this.currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        try
        {
            await this.currentTransaction?.RollbackAsync(ct)!;
        }
        finally
        {
            this.currentTransaction?.Dispose();
            this.currentTransaction = null;
        }
    }

    public Task ExecuteTransactionalAsync(CancellationToken ct = default)
    {
        var strategy = CreateExecutionStrategy();
        return strategy.ExecuteAsync(async () =>
        {
            await using var transaction =
                await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
            try
            {
                await SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        });
    }

    private void OnBeforeSaving()
    {
        try
        {
            foreach (var entry in ChangeTracker.Entries<IVersion>())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Entity.Version++;
                        break;

                    case EntityState.Deleted:
                        entry.Entity.Version++;
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("try for find IAggregate", ex);
        }
    }
}
