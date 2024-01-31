using MassTransit;

namespace RestaurantReservation.Domain.ReservationAggregate.Events;

public record UpdateReservationResult(
    ReservationId ReservationId,
    RestaurantId RestaurantId,
    TableId TableId,
    CustomerId CustomerId,
    ReservationStatus ReservationStatus,
    DateTime ReservationDate,
    ushort Occupants,
    ReviewId? ReviewId);

public record RequestUpdateReservation(
    Guid ReservationId,
    Guid RestaurantId,
    Guid TableId,
    Guid CustomerId,
    ReservationStatus ReservationStatus,
    DateTime ReservationDate,
    ushort Occupants,
    Guid? ReviewId);

public record ResponseUpdateReservation(
    Guid ReservationId,
    Guid RestaurantId,
    Guid TableId,
    Guid CustomerId,
    ReservationStatus ReservationStatus,
    DateTime ReservationDate,
    ushort Occupants,
    Guid? ReviewId);

public record UpdateReservation(
    ReservationId ReservationId,
    RestaurantId RestaurantId,
    TableId TableId,
    CustomerId CustomerId,
    ReservationStatus ReservationStatus,
    DateTime ReservationDate,
    ushort Occupants,
    ReviewId? ReviewId) : ICommand<UpdateReservationResult>, IEvent
{
    public Guid Id { get; } = NewId.NextGuid();
}

public record UpdateReservationDomainEvent(
    ReservationId ReservationId,
    RestaurantId RestaurantId,
    TableId TableId,
    CustomerId CustomerId,
    ReservationStatus ReservationStatus,
    DateTime ReservationDate,
    ushort Occupants,
    ReviewId? ReviewId) : IDomainEvent;
