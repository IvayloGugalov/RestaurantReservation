using MassTransit;
using RestaurantReservation.Core.CQRS;
using RestaurantReservation.Core.Event;
using RestaurantReservation.Domain.Common;
using RestaurantReservation.Domain.CustomerAggregate.ValueObjects;
using RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

namespace RestaurantReservation.Domain.RestaurantAggregate.Events;

public record CreateReviewResult(Guid Id);

public record CreateReviewEvent(
        int Rating,
        string Comment,
        RestaurantId RestaurantId,
        CustomerId CustomerId,
        string CustomerName)
    : ICommand<CreateReviewResult>, IEvent
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record ReviewCreatedDomainEvent(Review review) : IDomainEvent;
