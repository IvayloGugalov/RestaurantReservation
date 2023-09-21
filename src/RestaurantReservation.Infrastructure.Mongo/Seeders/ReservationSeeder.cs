namespace RestaurantReservation.Infrastructure.Mongo.Seeders;

public class ReservationSeeder : IDataSeeder
{
    private readonly AppMongoDbContext dbContext;

    public ReservationSeeder(AppMongoDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task SeedAllAsync()
    {
        if (!await this.dbContext.Reservations.AsQueryable().AnyAsync() &&
            await this.dbContext.Restaurants.AsQueryable().AnyAsync())
        {
            var reservations = Reservations().ToArray();

            await this.dbContext.Reservations.InsertManyAsync(reservations);
        }
    }

    private IEnumerable<Reservation> Reservations()
    {
        yield return Create(new ReservationId(Guid1), new RestaurantId(RestaurantSeeder.SavoryDelightsBistroId), new CustomerId(Guid.NewGuid()), DateTime.UtcNow, 4);
        yield return Create(new ReservationId(Guid2), new RestaurantId(RestaurantSeeder.SavoryDelightsBistroId), new CustomerId(Guid.NewGuid()), DateTime.UtcNow.AddMinutes(180), 2);
        yield return Create(new ReservationId(Guid3), new RestaurantId(RestaurantSeeder.GastronomiaEuphoriaRestaurantId), new CustomerId(Guid.NewGuid()), DateTime.UtcNow.AddDays(180), 5);
        yield return Create(new ReservationId(Guid4), new RestaurantId(RestaurantSeeder.GastronomiaEuphoriaRestaurantId), new CustomerId(Guid.NewGuid()), DateTime.UtcNow.AddHours(180), 3);
        yield return Create(new ReservationId(Guid5), new RestaurantId(RestaurantSeeder.GastronomiaEuphoriaRestaurantId), new CustomerId(Guid.NewGuid()), DateTime.UtcNow.AddMinutes(-180), 2);
        yield return Create(new ReservationId(Guid6), new RestaurantId(RestaurantSeeder.FusionFlareDinerId), new CustomerId(Guid.NewGuid()), DateTime.UtcNow.AddHours(90), 2);
        yield return Create(new ReservationId(Guid7), new RestaurantId(RestaurantSeeder.WhimsicalPlatesEateryId), new CustomerId(Guid.NewGuid()), DateTime.UtcNow.AddHours(-90), 8);
        yield return Create(new ReservationId(Guid8), new RestaurantId(RestaurantSeeder.CulinaryHavenCafeId), new CustomerId(Guid.NewGuid()), DateTime.UtcNow.AddHours(20), 4);
        yield return Create(new ReservationId(Guid9), new RestaurantId(RestaurantSeeder.WhimsicalPlatesEateryId), new CustomerId(Guid.NewGuid()), DateTime.UtcNow.AddDays(2), 2);
        yield return Create(new ReservationId(Guid10), new RestaurantId(RestaurantSeeder.CulinaryHavenCafeId), new CustomerId(Guid.NewGuid()), DateTime.UtcNow.AddDays(-88), 3);
    }

    private Reservation Create(
        ReservationId reservationId,
        RestaurantId restaurantId,
        CustomerId customerId,
        DateTime reservationTime,
        ushort occupants)
    {
        var table = this.dbContext.Tables.FindAsync(
                Builders<Table>.Filter.Eq(x => x.RestaurantId, restaurantId) & Builders<Table>.Filter.Gte(x => x.Capacity, occupants))
            .GetAwaiter().GetResult()
            .FirstOrDefault();

        if (table == null) throw new Exception("Table was not found for the given restaurant");

        return Reservation.Create(
            reservationId, restaurantId, table, customerId, reservationTime, occupants);
    }

    public static readonly Guid Guid1 = Guid.Parse("2b9ef999-07b6-4ea9-9a9b-7d763a6b7e01");
    public static readonly Guid Guid2 = Guid.Parse("4f2d3c4b-83c3-40f9-a42a-9907f34f11c8");
    public static readonly Guid Guid3 = Guid.Parse("a0bd6b5d-512f-44d3-9a41-aa6e4332976d");
    public static readonly Guid Guid4 = Guid.Parse("cc532feb-1a75-4de7-b963-87d440ac3aae");
    public static readonly Guid Guid5 = Guid.Parse("e5b8b79b-171b-45d0-a3ec-48233e648f79");
    public static readonly Guid Guid6 = Guid.Parse("36e6a6f0-e315-4dd9-82b5-180c0dfcd9f5");
    public static readonly Guid Guid7 = Guid.Parse("d4d9ca92-9d97-4bfe-8e86-55a536de1b0c");
    public static readonly Guid Guid8 = Guid.Parse("8f53b581-1a9c-4636-ba18-cf45f61e7464");
    public static readonly Guid Guid9 = Guid.Parse("fa2e693c-cc8b-4c67-9b21-4a16ab3d5d9f");
    public static readonly Guid Guid10 = Guid.Parse("c5625979-36de-4e80-9f95-0b36184a4160");
}
