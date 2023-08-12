using RestaurantReservation.Domain.CustomerAggregate.Events;

namespace RestaurantReservation.Domain.CustomerAggregate.Mappings;

public static class CustomerMappings
{
    public static CreateCustomer Map(RequestCreateCustomerDto model) =>
        new CreateCustomer(model.FirstName, model.LastName, model.Email);

    public static ResponseCreateCustomerDto Map(CreateCustomerResult model) =>
        new ResponseCreateCustomerDto(model.Id);
}
