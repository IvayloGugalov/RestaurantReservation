using MassTransit;

namespace RestaurantReservation.Domain.RestaurantAggregate.Events;

#region Single

public record AddTableResult(TableId TableId);

public record RequestAddTableDto(string Number, ushort Capacity, RestaurantId RestaurantId);

public record ResponseAddTableDto(Guid Id);

public record AddTable(string Number, ushort Capacity, RestaurantId RestaurantId)
    : ICommand<AddTableResult>, IEvent
{
    public Guid Id { get; } = NewId.NextGuid();
}

#endregion

#region Multiple

public record AddMultipleTablesResult(IEnumerable<TableId> TableIds);

public record RequestAddMultipleTablesDto(IEnumerable<(string Number, ushort Capacity)> TablesInformation, RestaurantId RestaurantId);

public record ResponseAddMultipleTablesDto(IEnumerable<Guid> Ids);

public record AddMultipleTables(IEnumerable<(string Number, ushort Capacity)> TableDtos, RestaurantId RestaurantId)
    : ICommand<AddMultipleTablesResult>, IEvent
{
    public Guid Id { get; } = NewId.NextGuid();
}

#endregion

