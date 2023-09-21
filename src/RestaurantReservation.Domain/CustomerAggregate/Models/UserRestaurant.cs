namespace RestaurantReservation.Domain.CustomerAggregate.Models;

public class UserRestaurant
{
    public CustomerId CustomerId { get; set; } = null!;
    // public Customer Customer { get; set; } = null!;

    public RestaurantId RestaurantId { get; set; } = null!;
    // public Restaurant Restaurant { get; set; } = null!;
}
