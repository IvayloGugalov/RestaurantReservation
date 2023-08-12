namespace RestaurantReservation.Domain.TableAggregate.ValueObjects;

public record TableId(Guid Value)
{
    public static implicit operator Guid(TableId tableId)
    {
        return tableId.Value;
    }
};