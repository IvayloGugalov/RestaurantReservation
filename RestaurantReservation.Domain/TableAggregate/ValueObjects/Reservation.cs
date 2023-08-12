using RestaurantReservation.Core.Model;
using RestaurantReservation.Domain.Common;
using RestaurantReservation.Domain.CustomerAggregate.ValueObjects;
using RestaurantReservation.Domain.TableAggregate.Exceptions;

namespace RestaurantReservation.Domain.TableAggregate.ValueObjects;

public record ReservationId(Guid Value)
{
    public static implicit operator Guid(ReservationId reservationId)
    {
        return reservationId.Value;
    }
};

public class Reservation : Entity<ReservationId>
{
    public static readonly TimeSpan MIN_STAY = TimeSpan.FromMinutes(90);

    public TableId TableId { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public DateTime ReservationDate { get; private set; }
    public ushort Occupants { get; private set; }

    private Reservation() { }

    public static Reservation Create(
        Table table,
        ReservationId reservationId,
        CustomerId customerId,
        DateTime reservationDate,
        ushort occupants)
    {
        if (table.Capacity < occupants) throw new TableLimitOfPeopleBreachedException(table.Capacity, occupants);
        if (table.Reservations.Any(r => r.ReservationDate.Add(MIN_STAY) < reservationDate)) throw new ReservationConflictException();

        var reservation = new Reservation
        {
            Id = reservationId,
            TableId = table.Id,
            CustomerId = customerId,
            ReservationDate = reservationDate,
            Occupants = occupants
        };

        return reservation;
    }
}
