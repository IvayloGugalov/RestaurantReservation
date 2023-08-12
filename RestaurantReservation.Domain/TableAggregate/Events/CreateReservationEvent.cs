using MassTransit;
using RestaurantReservation.Core.CQRS;
using RestaurantReservation.Core.Event;
using RestaurantReservation.Domain.Common;
using RestaurantReservation.Domain.CustomerAggregate.ValueObjects;
using RestaurantReservation.Domain.TableAggregate.ValueObjects;

namespace RestaurantReservation.Domain.TableAggregate.Events;

public record CreateReservationResult(Guid Id);

public record CreateReservationEvent(
        TableId TableId, CustomerId CustomerId, DateTime ReservationDate, ushort Occupants)
    : ICommand<CreateReservationResult>, IDomainEvent
{
    public Guid Id { get; } = NewId.NextGuid();
};

public record ReservationCreatedDomainEvent(
    ReservationId ReservationId, CustomerId CustomerId, TableId TableId, DateTime ReservationDate, ushort Occupants
) : IDomainEvent;
