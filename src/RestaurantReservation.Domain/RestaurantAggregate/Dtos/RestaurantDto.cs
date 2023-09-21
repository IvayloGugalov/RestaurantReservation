namespace RestaurantReservation.Domain.RestaurantAggregate.Dtos;

public record RestaurantDto(
    Guid Id, string Name, string Phone, string Description, string Url, string Website,
    WorkTimeDto WorkTime, IEnumerable<ReviewDto>? Reviews);
