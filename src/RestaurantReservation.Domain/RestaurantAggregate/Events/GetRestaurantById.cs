using RestaurantReservation.Domain.RestaurantAggregate.Dtos;

namespace RestaurantReservation.Domain.RestaurantAggregate.Events;

public record GetRestaurantById(Guid Id) : IQuery<GetRestaurantByIdResult>;

public record GetRestaurantByIdResult(RestaurantDto RestaurantDto);

public record GetRestaurantByIdResponseDto(RestaurantDto RestaurantDto);
