namespace RestaurantReservation.Domain.CustomerAggregate.Mappings;

public static class CustomerMappings
{
    public static CreateCustomer Map(RequestCreateCustomerDto model) =>
        new(model.FirstName, model.LastName, model.Email);

    public static ResponseCreateCustomerDto Map(CreateCustomerResult model) => new(model.Id);
}
