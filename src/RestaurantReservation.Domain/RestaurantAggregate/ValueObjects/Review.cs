namespace RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

public class Review : Entity<ReviewId>
{
    public Rating Rating { get; private init; } = null!;
    public string Comment { get; private init; } = null!;
    public RestaurantId RestaurantId { get; private init; } = null!;
    public CustomerId CustomerId { get; private init; } = null!;
    public string CustomerName { get; private init; } = null!;
    public ReservationId ReservationId { get; private init; } = null!;

    private Review() { }

    public static Review Create(
        ReviewId id,
        int ratingValue,
        string comment,
        RestaurantId restaurantId,
        CustomerId customerId,
        string customerName,
        ReservationId reservationId)
    {
        var newReview = new Review
        {
            Id = id,
            Rating = new Rating(ratingValue),
            Comment = comment,
            RestaurantId = restaurantId,
            CustomerId = customerId,
            CustomerName = customerName,
            ReservationId = reservationId
        };

        return newReview;
    }
}
