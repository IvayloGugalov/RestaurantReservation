namespace RestaurantReservation.Identity;

public static class Constants
{
    public static class Role
    {
        public const string Admin = "admin";
        public const string User = "user";
    }

    public static class StandardScopes
    {
        public const string Roles = "roles";
        public const string RestaurantReservationApi = "restaurant-reservation-api";
        public const string IdentityApi = "identity-api";
    }
}