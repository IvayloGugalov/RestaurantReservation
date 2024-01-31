using MassTransit;

namespace RestaurantReservation.Domain.ReservationAggregate.Events;

public record CreateReservationResult(Guid Id);

public record RequestCreateReservationDto(Guid TableId, Guid CustomerId, DateTime ReservationDate, ushort Occupants);

public record ResponseCreateReservationDto(Guid Id);

public record CreateReservation(
        Guid TableId, Guid CustomerId, DateTime ReservationDate, ushort Occupants)
    : ICommand<CreateReservationResult>, IEvent
{
    public Guid Id { get; } = NewId.NextGuid();
};

public record ReservationCreatedDomainEvent(
    ReservationId ReservationId, Guid CustomerId, Guid TableId, DateTime ReservationDate, ushort Occupants
) : IDomainEvent;
