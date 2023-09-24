namespace RestaurantReservation.Identity.DTOs;

public record RegisterNewUserRequestDto(
    string FirstName, string LastName, string Email,
    string Password, string ConfirmPassword);

public record RegisterNewUserResponseDto(Guid Id, string FirstName, string LastName);