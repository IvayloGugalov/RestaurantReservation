﻿using RestaurantReservation.Core.Exceptions;

namespace RestaurantReservation.Domain.CustomerAggregate.Exceptions;

public class CustomerAlreadyExistsException : CustomException
{
    public CustomerAlreadyExistsException(int? code = default)
        : base("Customer already exists", code: code)
    {
    }
}