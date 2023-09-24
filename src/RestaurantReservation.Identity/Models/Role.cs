namespace RestaurantReservation.Identity.Models;

public class Role: IdentityRole<Guid>, IVersion
{
    public long Version { get; set; }
}