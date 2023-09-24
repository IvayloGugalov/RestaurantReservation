using RestaurantReservation.Core.Mongo.Data;

namespace RestaurantReservation.Infrastructure.Mongo.Seeders;

public class CustomerSeeder : IDataSeeder
{
    private readonly AppMongoDbContext dbContext;

    public CustomerSeeder(AppMongoDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task SeedAllAsync()
    {
        if (!await this.dbContext.Customers.AsQueryable().AnyAsync())
        {
            await this.dbContext.Customers.InsertManyAsync(Customers());
            await this.dbContext.SaveChangesAsync();
        }
    }

    private static IEnumerable<Customer> Customers()
    {
        yield return Customer.Create(new CustomerId(JohnId), "John", "Smith", "johnsmith@abg.bg");
        yield return Customer.Create(new CustomerId(JaneId), "Jane", "Doe", "janedoe@abg.bg");
        yield return Customer.Create(new CustomerId(MichaelId), "Michael", "Johnson", "michaeljohnson@abg.bg");
        yield return Customer.Create(new CustomerId(EmilyId), "Emily", "Williams", "emilywilliams@abg.bg");
        yield return Customer.Create(new CustomerId(DavidId), "David", "Brown", "davidbrown@abg.bg");
    }

    public static readonly Guid JohnId = Guid.Parse("fc58a2c4-01e7-4d7e-85e1-0b6c3bea1831");
    public static readonly Guid JaneId = Guid.Parse("9d8c8a0b-5b48-4a8d-91d1-4cf34f76f811");
    public static readonly Guid MichaelId = Guid.Parse("7c5ea4a0-97a2-49a7-85b2-2d40657a5f3f");
    public static readonly Guid EmilyId = Guid.Parse("f147e287-2aef-4a82-99e0-9aae987ef20a");
    public static readonly Guid DavidId = Guid.Parse("b4db17bb-2f9e-40c0-baff-3e9d89499721");
}
