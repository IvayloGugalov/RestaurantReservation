using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RestaurantReservation.Identity.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityContext>
{
    public IdentityContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<IdentityContext>();

        builder.UseNpgsql("Server=localhost;Port=5432;Database=identity;User Id=postgres;Password=postgres;Include Error Detail=true");
        return new IdentityContext(builder.Options);
    }
}