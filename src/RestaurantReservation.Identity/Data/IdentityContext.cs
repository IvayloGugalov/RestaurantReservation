using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RestaurantReservation.Identity.Data;

public interface IDbContext
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public class IdentityContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>, IDbContext
{
    private readonly ILogger<IdentityContext>? logger;

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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.OnBeforeSaving();
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);

                if (databaseValues == null)
                {
                    this.logger?.LogError("The record no longer exists in the database, The record has been deleted by another user.");
                    throw;
                }

                // Refresh the original values to bypass next concurrency check
                entry.OriginalValues.SetValues(databaseValues);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
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
                    default: return;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("try for find IVersion", ex);
        }
    }
}