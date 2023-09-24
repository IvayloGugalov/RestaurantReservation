using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace RestaurantReservation.Core.Web;

public interface ICurrentUserProvider
{
    long? GetCurrentUserId();
}

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor httpContextAccessor;


    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public long? GetCurrentUserId()
    {
        var nameIdentifier = this.httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

        return long.TryParse(nameIdentifier?.Value, out var userId)
            ? userId
            : null;
    }
}
