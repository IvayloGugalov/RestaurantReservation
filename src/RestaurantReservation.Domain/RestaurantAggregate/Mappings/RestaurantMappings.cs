namespace RestaurantReservation.Domain.RestaurantAggregate.Mappings;

public static class RestaurantMappings
{
    public static CreateRestaurant Map(RequestCreateRestaurantDto model) =>
        new(model.Name, model.Phone, model.Description, model.Url, model.WebSite, model.WorkTime);

    public static ResponseCreateRestaurantDto Map(CreateRestaurantResult model) => new(model.Id);
}
