using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;

namespace RestaurantReservation.Identity.Configuration;

public class UserValidator : IResourceOwnerPasswordValidator
{
    private readonly SignInManager<User> signInManager;
    private readonly UserManager<User> userManager;

    public UserValidator(SignInManager<User> signInManager,
        UserManager<User> userManager)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var user = await this.userManager.FindByNameAsync(context.UserName);

        var signIn = await this.signInManager.PasswordSignInAsync(
            user!,
            context.Password,
            isPersistent: true,
            lockoutOnFailure: true);

        if (signIn.Succeeded)
        {
            var userId = user!.Id.ToString();

            // context set to success
            context.Result = new GrantValidationResult(
                subject: userId,
                authenticationMethod: "custom",
                claims: new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, userId),
                    new(ClaimTypes.Name, user.UserName!)
                }
            );

            return;
        }

        // context set to Failure
        context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient, "Invalid Credentials");
    }
}