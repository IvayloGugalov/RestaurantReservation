using Asp.Versioning.Builder;

namespace RestaurantReservation.Api.Endpoints;

public static class EndpointConfig
{
    public const string BaseApiPath = "api/v{version:apiVersion}";
    public static ApiVersionSet VersionSet { get; private set; } = default!;
}
