using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Infrastructure.Mongo.Exceptions;

public class CollectionNameDoesNotExist : CustomException
{
    public CollectionNameDoesNotExist(string collectionName, int? code = default)
        : base($"Collection name {collectionName} does not exist in the database", code: code)
    {
    }
}
