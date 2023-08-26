using MassTransit;
using RestaurantReservation.Domain.CustomerAggregate;

namespace RestaurantReservation.Domain.ReservationAggregate.Events;

public record CreateReservationResult(Guid Id);

public record CreateReservationEvent(
        TableId TableId, Customer Customer, DateTime ReservationDate, ushort Occupants)
    : ICommand<CreateReservationResult>, IDomainEvent
{
    public Guid Id { get; } = NewId.NextGuid();
};

public record ReservationCreatedDomainEvent(
    ReservationId ReservationId, CustomerId CustomerId, TableId TableId, DateTime ReservationDate, ushort Occupants
) : IDomainEvent;