namespace RestaurantReservation.Domain.RestaurantAggregate.Mappings;

public static class TableMappings
{
    #region Single

    public static AddTable Map(RequestAddTableDto model) =>
        new(model.Number, model.Capacity, model.RestaurantId);

    public static ResponseAddTableDto Map(AddTableResult model) => new(model.TableId);

    #endregion

    #region Multiple

    public static AddMultipleTables Map(RequestAddMultipleTablesDto model)
    {
        var tables = model.TablesInformation.Select(tableDto => (tableDto.Number, tableDto.Capacity));
        return new AddMultipleTables(tables, model.RestaurantId);
    }

    public static ResponseAddMultipleTablesDto Map(AddMultipleTablesResult model) =>
        new (model.TableIds.Select(x=> x.Value));

    #endregion


}
