using MassTransit;

namespace RestaurantReservation.Domain.RestaurantAggregate.Events;

public record AddTableResult(TableId TableId);

public record RequestAddTableDto(string Number, ushort Capacity, RestaurantId RestaurantId);

public record ResponseAddTableDto(Guid Id);

public record AddTable(string Number, ushort Capacity, RestaurantId RestaurantId)
    : ICommand<AddTableResult>, IEvent
{
    public Guid Id { get; } = NewId.NextGuid();
}
