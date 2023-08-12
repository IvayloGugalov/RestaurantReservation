namespace RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;

public record WorkTime(
    WorkingHours Monday,
    WorkingHours Tuesday,
    WorkingHours Wednesday,
    WorkingHours Thursday,
    WorkingHours Friday,
    WorkingHours Saturday,
    WorkingHours Sunday);

public record WorkingHours(TimeSpan OpeningTime, TimeSpan ClosingTime);
