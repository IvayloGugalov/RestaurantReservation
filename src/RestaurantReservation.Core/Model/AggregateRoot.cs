using RestaurantReservation.Core.Event;

namespace RestaurantReservation.Core.Model;

public abstract class AggregateRoot<TId, TIdType> : Entity<TId>, IAggregateRoot<TId>
    where TId : AggregateRootId<TIdType>
{
    public new AggregateRootId<TIdType> Id { get; protected init; } = null!;

    private readonly List<IDomainEvent> domainEvents = new();
    public virtual IReadOnlyList<IDomainEvent> DomainEvents => this.domainEvents.AsReadOnly();

    protected AggregateRoot () { }
    protected AggregateRoot(AggregateRootId<TIdType> id)
    {
        this.Id = id;
    }

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
