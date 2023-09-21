namespace RestaurantReservation.Core.Event;

public abstract class DomainEvent
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}