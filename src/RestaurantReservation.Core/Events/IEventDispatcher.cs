namespace RestaurantReservation.Core.Events;

public interface IEventDispatcher
{
    public Task SendAsync<T>(IReadOnlyList<T> events, Type? type = null, CancellationToken ct = default)
        where T : IEvent;
    public Task SendAsync<T>(T @event, Type? type = null, CancellationToken ct = default)
        where T : IEvent;
}
