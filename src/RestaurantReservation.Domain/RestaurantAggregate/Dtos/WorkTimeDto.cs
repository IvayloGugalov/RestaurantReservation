namespace RestaurantReservation.Domain.RestaurantAggregate.Dtos;

public record WorkTimeDto(
    string? MondayOpeningTime,
    string? MondayClosingTime,

    string? TuesdayOpeningTime,
    string? TuesdayClosingTime,

    string? WednesdayOpeningTime,
    string? WednesdayClosingTime,

    string? ThursdayOpeningTime,
    string? ThursdayClosingTime,

    string? FridayOpeningTime,
    string? FridayClosingTime,

    string? SaturdayOpeningTime,
    string? SaturdayClosingTime,

    string? SundayOpeningTime,
    string? SundayClosingTime);
