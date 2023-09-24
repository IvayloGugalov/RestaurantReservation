using RestaurantReservation.Identity.Validations;

namespace RestaurantReservation.Identity.Services;

public class UserService : IUserService
{
    // private readonly IEventDispatcher _eventDispatcher;
    private readonly UserManager<User> userManager;

    public UserService(UserManager<User> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<RegisterNewUserResponseDto> RegisterNewUserAsync(RegisterNewUserRequestDto request,
        CancellationToken cancellationToken)
    {
        var validations = await new RegisterNewUserValidator().ValidateAsync(request);
        if (!validations.IsValid)
        {
            throw new ValidationException(validations.Errors.First().ErrorMessage);
        }

        var applicationUser = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = request.Password,
        };

        var identityResult = await this.userManager.CreateAsync(applicationUser, request.Password);
        var roleResult = await this.userManager.AddToRoleAsync(applicationUser, Constants.Role.User);

        if (identityResult.Succeeded == false)
        {
            throw new RegisterIdentityUserException(string.Join(',', identityResult.Errors.Select(e => e.Description)));
        }

        if (roleResult.Succeeded == false)
        {
            throw new RegisterIdentityUserException(string.Join(',', roleResult.Errors.Select(e => e.Description)));
        }

        return new RegisterNewUserResponseDto(applicationUser.Id, applicationUser.FirstName, applicationUser.LastName);
    }
}