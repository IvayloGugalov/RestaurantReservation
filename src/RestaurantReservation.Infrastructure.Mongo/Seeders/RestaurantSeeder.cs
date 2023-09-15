namespace RestaurantReservation.Infrastructure.Mongo.Seeders;

public class RestaurantSeeder : IDataSeeder
{
    private readonly AppMongoDbContext dbContext;

    public RestaurantSeeder(AppMongoDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task SeedAllAsync()
    {
        if (!await this.dbContext.Restaurants.AsQueryable().AnyAsync())
        {
            // await this.dbContext.BeginTransactionAsync();
            var restaurants = Restaurants().ToArray();

            foreach (var restaurant in restaurants)
            {
                if (restaurant.Id.Value == SavoryDelightsBistroId)
                {
                    restaurant.SetWorkTime(CreateWorkTime(9, 17, 10, 16));
                }
                else if (restaurant.Id.Value == WhimsicalPlatesEateryId)
                {
                    restaurant.SetWorkTime(CreateWorkTime(11, 22, 11, 23));
                }
                else if (restaurant.Id.Value == CulinaryHavenCafeId)
                {
                    restaurant.SetWorkTime(CreateWorkTime(8, 16, 9, 14));
                }
                else if (restaurant.Id.Value == FusionFlareDinerId)
                {
                    restaurant.SetWorkTime(WorkTime.CreateEmpty());
                }
                else if (restaurant.Id.Value == GastronomiaEuphoriaRestaurantId)
                {
                    restaurant.SetWorkTime(CreateWorkTime(10, 20, 10, 22));
                }
            }
            await this.dbContext.Restaurants.InsertManyAsync(restaurants);

            // await this.dbContext.CommitTransactionAsync();

            foreach (var restaurant in restaurants)
            {
                AddTables(restaurant);
            }
            // await this.dbContext.SaveChangesAsync();
        }
    }

    private void AddTables(Restaurant restaurant)
    {
        var tables = new List<Table>();
        for (var i = 1; i <= 10; i++)
        {
            var tableId = new TableId(Guid.NewGuid());
            tables.Add(restaurant.AddTable(tableId, i.ToString(), 4));
        }
        this.dbContext.Tables.InsertMany(tables);
    }

    private static IEnumerable<Restaurant> Restaurants()
    {
        yield return Restaurant.Create(new RestaurantId(SavoryDelightsBistroId), "Savory Delights Bistro", "123-456-7890", "Description 1", "www.example.com", "www.website1.com");
        yield return Restaurant.Create(new RestaurantId(WhimsicalPlatesEateryId), "Whimsical Plates Eatery", "987-654-3210", "Description 2", "www.example.com", "www.website2.com");
        yield return Restaurant.Create(new RestaurantId(CulinaryHavenCafeId), "Culinary Haven Cafe", "555-123-4567", "Description 3", "www.example.com", "www.website3.com");
        yield return Restaurant.Create(new RestaurantId(FusionFlareDinerId), "FusionFlare Diner", "777-888-9999", "Description 4", "www.example.com", "www.website4.com");
        yield return Restaurant.Create(new RestaurantId(GastronomiaEuphoriaRestaurantId), "Gastronomia Euphoria Restaurant", "111-222-3333", "Description 5", "www.example.com", "www.website5.com");
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
