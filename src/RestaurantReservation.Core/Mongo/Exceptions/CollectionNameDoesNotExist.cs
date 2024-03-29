﻿using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Core.Mongo.Exceptions;

public class CollectionNameDoesNotExist : NotFoundException
{
    public CollectionNameDoesNotExist(string collectionName, int? code = default)
        : base($"Collection name {collectionName} does not exist in the database", code: code)
    {
    }
}
