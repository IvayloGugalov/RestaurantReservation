﻿namespace RestaurantReservation.Core.Exceptions;

public class BadRequestException : CustomException
{
    public BadRequestException(string message, int? code = null) : base(message, code: code)
    {
    }
}