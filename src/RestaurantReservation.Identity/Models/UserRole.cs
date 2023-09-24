namespace RestaurantReservation.Identity.Models;

public class UserRole: IdentityUserRole<Guid>, IVersion
{
    public long Version { get; set; }
}