using Microsoft.EntityFrameworkCore;

namespace RestaurantReservation.Infrastructure.EF.Data.Seeders;

public class RestaurantSeeder : IDataSeeder
{
    private readonly AppDbContext dbContext;

    public RestaurantSeeder(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task SeedAllAsync()
    {
        if (!await this.dbContext.Restaurants.AnyAsync())
        {
            await this.dbContext.Restaurants.AddRangeAsync(Restaurants());
            await this.dbContext.SaveChangesAsync();

            foreach (var restaurant in this.dbContext.Restaurants)
            {
                AddTables(restaurant);
            }
            await this.dbContext.SaveChangesAsync();
        }
    }

    private static void AddTables(Restaurant restaurant)
    {
        for (var i = 1; i <= 10; i++)
        {
            var tableId = new TableId(Guid.NewGuid());
            restaurant.AddTable(tableId, i.ToString(), 4);
        }
    }

    private static IEnumerable<Restaurant> Restaurants()
    {
        yield return Restaurant.Create(new RestaurantId(SavoryDelightsBistroId), "Savory Delights Bistro", "123-456-7890", "Description 1", "www.example.com", "www.website1.com", CreateWorkTime(9, 17, 10, 16));
        yield return Restaurant.Create(new RestaurantId(WhimsicalPlatesEateryId), "Whimsical Plates Eatery", "987-654-3210", "Description 2", "www.example.com", "www.website2.com", CreateWorkTime(11, 22, 11, 23));
        yield return Restaurant.Create(new RestaurantId(CulinaryHavenCafeId), "Culinary Haven Cafe", "555-123-4567", "Description 3", "www.example.com", "www.website3.com", CreateWorkTime(8, 16, 9, 14));
        yield return Restaurant.Create(new RestaurantId(FusionFlareDinerId), "FusionFlare Diner", "777-888-9999", "Description 4", "www.example.com", "www.website4.com", WorkTime.CreateEmpty());
        yield return Restaurant.Create(new RestaurantId(GastronomiaEuphoriaRestaurantId), "Gastronomia Euphoria Restaurant", "111-222-3333", "Description 5", "www.example.com", "www.website5.com", CreateWorkTime(10, 20, 10, 22));
    }

    private static WorkTime CreateWorkTime(int weekStart, int weekEnd, int weekendStart, int weekendEnd)
    {
        var weekdays = WorkingHours.Create(TimeSpan.FromHours(weekStart), TimeSpan.FromHours(weekEnd));
        var weekends = WorkingHours.Create(TimeSpan.FromHours(weekendStart), TimeSpan.FromHours(weekendEnd));

        return WorkTime.Create(weekdays, weekdays, weekdays, weekdays, weekdays, weekends, weekends);
    }

    public static readonly Guid SavoryDelightsBistroId = Guid.Parse("fc58a2c4-01e7-4d7e-85e1-0b6c3bea1831");
    public static readonly Guid WhimsicalPlatesEateryId = Guid.Parse("9d8c8a0b-5b48-4a8d-91d1-4cf34f76f811");
    public static readonly Guid CulinaryHavenCafeId = Guid.Parse("7c5ea4a0-97a2-49a7-85b2-2d40657a5f3f");
    public static readonly Guid FusionFlareDinerId = Guid.Parse("f147e287-2aef-4a82-99e0-9aae987ef20a");
    public static readonly Guid GastronomiaEuphoriaRestaurantId = Guid.Parse("b4db17bb-2f9e-40c0-baff-3e9d89499721");

}
