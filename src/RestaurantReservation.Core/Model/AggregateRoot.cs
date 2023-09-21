using RestaurantReservation.Core.Event;

namespace RestaurantReservation.Core.Model;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId>
    where TId : IEquatable<TId>
{
    private readonly List<IDomainEvent> domainEvents = new();
    public virtual IReadOnlyList<IDomainEvent> DomainEvents => this.domainEvents.AsReadOnly();

    protected virtual void AddDomainEvent(IDomainEvent newEvent)
    {
        this.domainEvents.Add(newEvent);
    }

    public virtual IEvent[] ClearDomainEvents()
    {
        IEvent[] dequeuedEvents = this.domainEvents.ToArray();
        this.domainEvents.Clear();
        return dequeuedEvents;
    }
}
