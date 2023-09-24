namespace RestaurantReservation.Identity.Models;

public class UserToken: IdentityUserToken<Guid>, IVersion
{
    public long Version { get; set; }
}