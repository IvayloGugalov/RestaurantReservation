using Asp.Versioning.Builder;

namespace RestaurantReservation.Api.Endpoints;

public sealed class EndpointConfig
{
    public const string BaseApiPath = "api/v{version:apiVersion}";
    public static ApiVersionSet VersionSet { get; private set; } = default!;
}
