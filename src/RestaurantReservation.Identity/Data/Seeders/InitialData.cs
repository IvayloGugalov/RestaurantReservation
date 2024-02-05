using MassTransit;

namespace RestaurantReservation.Identity.Data.Seeders;

public static class InitialData
{
    public static List<User> Users { get; }

    static InitialData()
    {
        Users = new List<User>
        {
            new()
            {
                Id = NewId.NextGuid(),
                FirstName = "Samantha",
                LastName = "Smith",
                UserName = "S_Smith",
                Email = "sam_smith@test.com",
                SecurityStamp = Guid.NewGuid().ToString()
            },
            new()
            {
                Id = NewId.NextGuid(),
                FirstName = "John",
                LastName = "Smith",
                UserName = "J_Smith",
                Email = "john_smith@test.com",
                SecurityStamp = Guid.NewGuid().ToString()
            }
        };
    }
}