﻿using System.Collections.Immutable;
using System.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using RestaurantReservation.Core.EFCore;
using RestaurantReservation.Core.Events;
using RestaurantReservation.Core.Model;
using RestaurantReservation.Domain.CustomerAggregate.Models;

namespace RestaurantReservation.Infrastructure.EF.Data;

public class AppDbContext : DbContext, IDbContext
{
    private readonly ILogger<AppDbContext>? logger;
    private IDbContextTransaction? currentTransaction;

    public IExecutionStrategy CreateExecutionStrategy() => this.Database.CreateExecutionStrategy();

    public DbSet<Restaurant> Restaurants { get; set; } = null!;
    public DbSet<Reservation> Reservations { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;

    public AppDbContext(DbContextOptions options, ILogger<AppDbContext>? logger = null)
        : base(options)
    {
        this.logger = logger;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
        // builder.FilterSoftDeletedProperties();
        // builder.ToSnakeCaseTables();
    }

    // TODO: If going to have a base DbContext and split the aggregates into separate Microservices
    public DbSet<TEntity> Set<TEntity, TId>()
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId> =>
        base.Set<TEntity>();

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        if (this.currentTransaction != null) return;

        this.currentTransaction =
            await this.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        try
        {
            await this.SaveChangesAsync(ct);
            await this.currentTransaction?.CommitAsync(ct)!;
        }
        catch
        {
            await this.RollbackTransactionAsync(ct);
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
        var strategy = this.CreateExecutionStrategy();
        return strategy.ExecuteAsync(async () =>
        {
            await using var transaction =
                await this.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
            try
            {
                await this.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        });
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
                    this.logger?.LogError(
                        "The record no longer exists in the database, The record has been deleted by another user.");
                    throw;
                }

                // Refresh the original values to bypass next concurrency check
                entry.OriginalValues.SetValues(databaseValues);
            }

            return await base.SaveChangesAsync(ct);
        }
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

    // ref: https://www.meziantou.net/entity-framework-core-generate-tracking-columns.htm
    // ref: https://www.meziantou.net/entity-framework-core-soft-delete-using-query-filters.htm
    private void OnBeforeSaving()
    {
        try
        {
            foreach (var entry in this.ChangeTracker.Entries<IAggregateRoot>())
            {
                var isAuditable = entry.Entity.GetType().IsAssignableTo(typeof(IAggregateRoot));
                // var userId = _currentUserProvider?.GetCurrentUserId() ?? 0;

                if (isAuditable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            // entry.Entity.CreatedBy = userId;
                            entry.Entity.CreatedAt = DateTime.UtcNow;
                            break;

                        case EntityState.Modified:
                            // entry.Entity.LastModifiedBy = userId;
                            entry.Entity.LastModified = DateTime.UtcNow;
                            break;

                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            // entry.Entity.LastModifiedBy = userId;
                            entry.Entity.LastModified = DateTime.UtcNow;
                            entry.Entity.IsDeleted = true;
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("try for find IAggregate", ex);
        }
    }
}
