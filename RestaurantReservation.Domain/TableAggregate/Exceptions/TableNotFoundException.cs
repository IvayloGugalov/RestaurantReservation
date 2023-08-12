using RestaurantReservation.Core.Exceptions;
using RestaurantReservation.Domain.Common;

namespace RestaurantReservation.Domain.TableAggregate.Exceptions;

public class TableNotFoundException : CustomException
{
    public TableNotFoundException(int? code = default)
        : base("Table not found", code: code)
    {
    }
}
