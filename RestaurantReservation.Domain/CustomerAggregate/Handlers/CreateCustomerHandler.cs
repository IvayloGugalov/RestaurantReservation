using RestaurantReservation.Core.CQRS;
using RestaurantReservation.Core.Repository;
using RestaurantReservation.Domain.CustomerAggregate.Events;
using RestaurantReservation.Domain.CustomerAggregate.Exceptions;
using RestaurantReservation.Domain.CustomerAggregate.ValueObjects;

namespace RestaurantReservation.Domain.CustomerAggregate.Handlers;

public class CreateCustomerHandler : ICommandHandler<CreateCustomer, CreateCustomerResult>
{
    private readonly IRepositoryBase<Customer, CustomerId> customerRepository;

    public CreateCustomerHandler(IRepositoryBase<Customer, CustomerId> customerRepository)
    {
        this.customerRepository = customerRepository;
    }

    public async Task<CreateCustomerResult> Handle(CreateCustomer request, CancellationToken cancellationToken)
    {
        var customer = await this.customerRepository.FirstOrDefaultAsync(x => request.Email == x.Email.Value, cancellationToken);
        if (customer != null) throw new CustomerAlreadyExistsException();

        var customerEntity = Customer.Create(
            new CustomerId(request.Id),
            firstName: request.FirstName,
            lastName: request.LastName,
            emailValue: request.Email);

        var newCustomer = await this.customerRepository.AddAsync(customerEntity, cancellationToken);
        return new CreateCustomerResult(newCustomer.Id);
    }
}
