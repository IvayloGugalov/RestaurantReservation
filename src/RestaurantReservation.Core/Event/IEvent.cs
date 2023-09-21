using MassTransit;
using MediatR;

namespace RestaurantReservation.Core.Event;

public interface IEvent : INotification
{
    Guid EventId => NewId.NextGuid();
    public DateTime OccurredOn => DateTime.Now;
    public string EventType => GetType().AssemblyQualifiedName ?? throw new InvalidOperationException();
}