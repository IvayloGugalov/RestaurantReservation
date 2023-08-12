using MassTransit;
using RestaurantReservation.Core.CQRS;
using RestaurantReservation.Core.Event;
using RestaurantReservation.Domain.Common;
using RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

namespace RestaurantReservation.Domain.CustomerAggregate.Events;

public record AddRestaurantToFavoritesResult(Guid Id);

public record AddRestaurantToFavoritesEvent(Customer Customer, RestaurantId RestaurantId)
    : ICommand<AddRestaurantToFavoritesResult>, IEvent
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record RestaurantAddedToFavoritesDomainEvent(RestaurantId RestaurantId) : IDomainEvent;
