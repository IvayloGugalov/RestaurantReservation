namespace RestaurantReservation.Core.Model;

public abstract record AggregateRootId<TId>
{
    public abstract TId Value { get; protected set; }
}
