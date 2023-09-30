using RestaurantReservation.Core.Events;

namespace RestaurantReservation.Identity.Contracts;

public record UserCreated(Guid Id, string FullName) : IIntegrationEvent;
