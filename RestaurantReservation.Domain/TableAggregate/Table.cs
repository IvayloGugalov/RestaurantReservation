using RestaurantReservation.Core.Model;
using RestaurantReservation.Domain.Common;
using RestaurantReservation.Domain.CustomerAggregate.ValueObjects;
using RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;
using RestaurantReservation.Domain.TableAggregate.Events;
using RestaurantReservation.Domain.TableAggregate.ValueObjects;

namespace RestaurantReservation.Domain.TableAggregate;

public class Table : AggregateRoot<TableId>
{
    public string Number { get; private set; }
    public ushort Capacity { get; private set; }
    public RestaurantId RestaurantId { get; private set; }

    private readonly List<Reservation> reservations;
    public IReadOnlyCollection<Reservation> Reservations { get; set; }

    private Table()
    {
        this.reservations = new List<Reservation>();
    }

    public static Table Create(
        TableId id,
        string number,
        ushort capacity,
        RestaurantId restaurantId)
    {
        var table = new Table
        {
            Id = id,
            Number = number,
            Capacity = capacity,
            RestaurantId = restaurantId
        };

        return table;
    }

    public Reservation AddReservation(
        ReservationId reservationId,
        CustomerId customerId,
        DateTime reservationDate,
        ushort occupants)
    {
        var reservation = Reservation.Create(
            this,
            reservationId,
            customerId,
            reservationDate,
            occupants);

        this.reservations.Add(reservation);

        var @event = new ReservationCreatedDomainEvent(
            reservation.Id, reservation.CustomerId, reservation.TableId, reservation.ReservationDate, reservation.Occupants);
        this.AddDomainEvent(@event);

        return reservation;
    }

    public void RemoveReservation(ReservationId reservationId)
    {
        var reservation = this.reservations.SingleOrDefault(x => x.Id == reservationId);
        if (reservation == null) return;

        this.reservations.Remove(reservation);
    }
}
