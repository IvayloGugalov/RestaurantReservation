namespace RestaurantReservation.Identity.Models;

public class UserLogin: IdentityUserLogin<Guid>, IVersion
{
    public long Version { get; set; }
}