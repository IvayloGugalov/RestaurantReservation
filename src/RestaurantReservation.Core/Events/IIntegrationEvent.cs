using MassTransit;

namespace RestaurantReservation.Core.Events;

[ExcludeFromTopology]
public interface IIntegrationEvent : IEvent
{
}
