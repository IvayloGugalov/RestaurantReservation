namespace RestaurantReservation.Domain.RestaurantAggregate.Mappings;

public static class TableMappings
{
    public static AddTable Map(RequestAddTableDto model) =>
        new(model.Number, model.Capacity, model.RestaurantId);

    public static ResponseAddTableDto Map(AddTableResult model) => new(model.TableId);
}
