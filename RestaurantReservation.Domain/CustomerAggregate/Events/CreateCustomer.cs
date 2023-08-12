using MassTransit;
using RestaurantReservation.Core.CQRS;
using RestaurantReservation.Core.Event;

namespace RestaurantReservation.Domain.CustomerAggregate.Events;

public record CreateCustomerResult(Guid Id);

public record RequestCreateCustomerDto(string FirstName, string LastName, string Email);
public record ResponseCreateCustomerDto(Guid Id);

public record CreateCustomer(string FirstName, string LastName, string Email)
    : ICommand<CreateCustomerResult>
{
    public Guid Id { get; } = NewId.NextGuid();
}

public record CustomerCreatedDomainEvent
    (Guid Id, string FirstName, string LastName, string Email) : IDomainEvent;
