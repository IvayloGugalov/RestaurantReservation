namespace RestaurantReservation.Domain.CustomerAggregate.Events;

public record RestaurantRemovedFromFavorites(RestaurantId restaurantId) : IDomainEvent;
