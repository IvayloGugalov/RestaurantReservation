namespace RestaurantReservation.Domain.ReservationAggregate.Models;

public record Rating
{
    public int Value { get; }

    private Rating()
    {
    }

    public Rating(int value)
        : base()
    {
        if (value is < 0 or > 5) throw new ArgumentException("Invalid Rating value provided");
        this.Value = value;
    }
}