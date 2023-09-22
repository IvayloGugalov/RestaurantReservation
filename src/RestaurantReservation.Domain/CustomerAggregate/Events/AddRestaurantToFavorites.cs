using MassTransit;

namespace RestaurantReservation.Domain.CustomerAggregate.Events;

public record AddRestaurantToFavoritesResult(Guid Id);

public record AddRestaurantToFavoritesEvent(Customer Customer, RestaurantId RestaurantId)
    : ICommand<AddRestaurantToFavoritesResult>, IEvent
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record RestaurantAddedToFavoritesDomainEvent(RestaurantId RestaurantId) : IDomainEvent;
