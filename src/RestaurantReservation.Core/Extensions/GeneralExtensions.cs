namespace RestaurantReservation.Core.Extensions;

public static class GeneralExtensions
{
    public static bool IsAny<T>(this T obj, params T[] args)
    {
        return Array.IndexOf(args, obj) >= 0;
    }
}
