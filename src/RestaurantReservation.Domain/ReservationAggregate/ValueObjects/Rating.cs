namespace RestaurantReservation.Domain.ReservationAggregate.ValueObjects;

public record Rating
{
    public double Value { get; }

    public Rating(double value)
    {
        if (value is < 0 or > 5) throw new ArgumentException("Invalid Rating value provided");
        this.Value = value;
    }
}
