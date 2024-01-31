namespace RestaurantReservation.Domain.ReservationAggregate.Models;

public class Reservation : AggregateRoot<ReservationId>
{
    public static readonly TimeSpan MIN_STAY = TimeSpan.FromMinutes(90);

    public RestaurantId RestaurantId { get; private set; } = null!;
    public CustomerId CustomerId { get; private set; } = null!;
    public TableId TableId { get; private set; } = null!;
    public DateTime ReservationDate { get; private set; }
    public ushort Occupants { get; private set; }
    public ReservationStatus Status { get; private set; }
    public ReviewId? ReviewId { get; private set; }

    private Reservation() { }

    public static Reservation Create(
        ReservationId reservationId,
        RestaurantId restaurantId,
        Table table,
        CustomerId customerId,
        DateTime reservationDate,
        ushort occupants)
    {
        CanUseTableForReservation(table, reservationDate, occupants);

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

    public void Update(
        ReservationId reservationId,
        RestaurantId restaurantId,
        Table table,
        CustomerId customerId,
        ReservationStatus reservationStatus,
        DateTime reservationDate,
        ushort occupants,
        ReviewId? reviewId)
    {
        CanUseTableForReservation(table, reservationDate, occupants);

        this.Id = reservationId;
        this.RestaurantId = restaurantId;
        this.TableId = table.Id;
        this.CustomerId = customerId;
        this.ReservationDate = reservationDate;
        this.Occupants = occupants;
        this.ReviewId = reviewId;

        if (this.Status != reservationStatus && reservationStatus > this.Status)
        {
            this.Status = reservationStatus;
        }

        var @event = new UpdateReservationDomainEvent(
            reservationId,
            restaurantId,
            table.Id,
            customerId,
            reservationStatus,
            reservationDate,
            occupants,
            reviewId);

        this.AddDomainEvent(@event);
    }

    private static void CanUseTableForReservation(Table table, DateTime reservationDate, ushort occupants)
    {
        if (table.Capacity < occupants) throw new TableLimitOfPeopleBreachedException(table.Capacity, occupants);
        if (table.Reservations.All(r => r.ReservationDate.Add(MIN_STAY) < reservationDate)) throw new ReservationConflictException();
    }
}
