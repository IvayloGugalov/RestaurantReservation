namespace RestaurantReservation.Domain.ReservationAggregate.Models;

public class Reservation : AggregateRoot<ReservationId>
{
    public static readonly TimeSpan MIN_STAY = TimeSpan.FromMinutes(90);

    public RestaurantId RestaurantId { get; set; } = null!;
    public Guid CustomerId { get; private init; }
    public Guid TableId { get; private init; }
    public DateTime ReservationDate { get; private init; }
    public ushort Occupants { get; private init; }
    public ReservationStatus Status { get; private set; }
    public Guid? ReviewId { get; set; }

    private Reservation() { }

    public static Reservation Create(
        ReservationId reservationId,
        RestaurantId restaurantId,
        Table table,
        Guid customerId,
        DateTime reservationDate,
        ushort occupants)
    {
        if (table.Capacity < occupants) throw new TableLimitOfPeopleBreachedException(table.Capacity, occupants);
        if (table.Reservations.All(r => r.ReservationDate.Add(MIN_STAY) < reservationDate))
            throw new ReservationConflictException();

        var reservation = new Reservation
        {
            Id = reservationId,
            TableId = table.Id,
            RestaurantId = restaurantId,
            CustomerId = customerId,
            ReservationDate = reservationDate,
            Occupants = occupants,
            Status = ReservationStatus.Created
        };

        return reservation;
    }

    // TODO: Make a generic ReservationUpdater method
    public void Confirm()
    {
        // Add domain-specific logic to confirm the reservation
        // e.g., Check availability, update status, etc.

        this.Status = ReservationStatus.Confirmed;

        // Raise domain events or perform other actions
    }

    public void Cancel()
    {
        // Add domain-specific logic to cancel the reservation
        // e.g., Handle cancellation policies, update status, etc.

        this.Status = ReservationStatus.Canceled;

        // Raise domain events or perform other actions
    }

    public void CompleteReservation()
    {
    }
}
