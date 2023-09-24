namespace RestaurantReservation.Identity.Models;

public class UserClaim: IdentityUserClaim<Guid>, IVersion
{
    public long Version { get; set; }
}