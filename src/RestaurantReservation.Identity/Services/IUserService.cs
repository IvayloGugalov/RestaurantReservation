namespace RestaurantReservation.Identity.Services;

public interface IUserService
{
    Task<RegisterNewUserResponseDto> RegisterNewUserAsync(RegisterNewUserRequestDto request,
        CancellationToken ct);

    Task<List<User>> GetUsersAsync();
}
