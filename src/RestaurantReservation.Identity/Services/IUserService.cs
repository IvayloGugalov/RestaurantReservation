namespace RestaurantReservation.Identity.Services;

public interface IUserService
{
    Task<RegisterNewUserResponseDto> RegisterNewUserAsync(RegisterNewUserRequestDto request,
        CancellationToken cancellationToken);

    Task<List<User>> GetUsersAsync();
}
