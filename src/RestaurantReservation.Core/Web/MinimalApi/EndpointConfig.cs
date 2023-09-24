using Asp.Versioning.Builder;

namespace RestaurantReservation.Core.Web.MinimalApi;

public static class EndpointConfig
{
    public const string BaseApiPath = "api/v{version:apiVersion}";
    public static ApiVersionSet VersionSet { get; private set; } = default!;
}
