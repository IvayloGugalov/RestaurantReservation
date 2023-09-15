using RestaurantReservation.Core.Event;

namespace RestaurantReservation.Core.Model;

public interface IAggregateRoot<T> : IAggregateRoot, IEntity<T> where T : IEquatable<T>
{
}

public interface IAggregateRoot : IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IEvent[] ClearDomainEvents();
}
