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
        var validations = await new RegisterNewUserValidator().ValidateAsync(request, cancellationToken);
        if (!validations.IsValid)
        {
            throw new ValidationException(string.Join(',', validations.Errors.Select(e => e.ErrorMessage)));
        }

        var applicationUser = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = request.Password,
            UserName = $"{request.FirstName}{request.LastName}"
        };

        var identityResult = await this.userManager.CreateAsync(applicationUser, request.Password);
        var roleResult = await this.userManager.AddToRoleAsync(applicationUser, Constants.Role.User);

        if (!identityResult.Succeeded)
        {
            throw new RegisterIdentityUserException(string.Join(',', identityResult.Errors.Select(e => e.Description)));
        }

        if (!roleResult.Succeeded)
        {
            throw new RegisterIdentityUserException(string.Join(',', roleResult.Errors.Select(e => e.Description)));
        }

        return new RegisterNewUserResponseDto(applicationUser.Id, applicationUser.FirstName, applicationUser.LastName);
    }

    public Task<List<User>> GetUsersAsync()
    {
        return Task.FromResult(this.userManager.Users.ToList());
    }
}
