namespace RestaurantReservation.Identity.Models;

public class RoleClaim: IdentityRoleClaim<Guid>, IVersion
{
    public long Version { get; set; }
}