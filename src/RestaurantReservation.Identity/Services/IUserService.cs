using RestaurantReservation.Identity.DTOs;

namespace RestaurantReservation.Identity.Services;

public interface IUserService
{
    Task<RegisterNewUserResponseDto> RegisterNewUserAsync(RegisterNewUserRequestDto request);
}