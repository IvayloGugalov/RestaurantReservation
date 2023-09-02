using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace RestaurantReservation.Infrastructure.EF.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();

        builder.UseNpgsql(
                "Server=localhost;Port=5432;Database=restaurant_reservation;User Id=postgres;Password=postgres;Include Error Detail=true")
            .UseSnakeCaseNamingConvention()
            .LogTo(Console.WriteLine, LogLevel.Debug);
        return new AppDbContext(builder.Options);
    }
}
