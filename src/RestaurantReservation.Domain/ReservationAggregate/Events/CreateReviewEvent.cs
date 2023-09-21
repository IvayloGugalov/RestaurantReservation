using MassTransit;

namespace RestaurantReservation.Domain.ReservationAggregate.Events;

public record CreateReviewResult(Guid Id);

public record CreateReviewEvent(
        ReservationId ReservationId,
        int Rating,
        string Comment,
        string CustomerName)
    : ICommand<CreateReviewResult>, IEvent
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record ReviewCreatedDomainEvent(Review review) : IDomainEvent;