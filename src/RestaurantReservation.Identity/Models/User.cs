namespace RestaurantReservation.Identity.Models;

public class User : IdentityUser<Guid>, IVersion
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public long Version { get; set; }
}