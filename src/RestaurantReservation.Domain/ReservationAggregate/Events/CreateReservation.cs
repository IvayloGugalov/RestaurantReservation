using MassTransit;

namespace RestaurantReservation.Domain.ReservationAggregate.Events;

public record CreateReservationResult(Guid Id);

public record RequestCreateReservationDto(TableId TableId, CustomerId CustomerId, DateTime ReservationDate, ushort Occupants);

public record ResponseCreateReservationDto(Guid Id);

public record CreateReservation(
        TableId TableId, CustomerId CustomerId, DateTime ReservationDate, ushort Occupants)
    : ICommand<CreateReservationResult>, IDomainEvent
{
    public Guid Id { get; } = NewId.NextGuid();
};

public record ReservationCreatedDomainEvent(
    ReservationId ReservationId, Guid CustomerId, Guid TableId, DateTime ReservationDate, ushort Occupants
) : IDomainEvent;
