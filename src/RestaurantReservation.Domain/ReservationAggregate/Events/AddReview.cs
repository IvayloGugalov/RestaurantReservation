using MassTransit;

namespace RestaurantReservation.Domain.ReservationAggregate.Events;

public record CreateReviewResult(Guid Id);

public record RequestAddReviewDto(
    Guid ReservationId,
    int Rating,
    string Comment,
    string CustomerName
);

public record ResponseAddReviewDto(Guid Id);

public record AddReview(
    Guid ReservationId,
    int Rating,
    string Comment,
    string CustomerName) : ICommand<CreateReviewResult>, IEvent
{
    public Guid Id { get; } = NewId.NextGuid();
}

public record ReviewCreatedDomainEvent(Review Review) : IDomainEvent;
