using RestaurantReservation.Domain.CustomerAggregate.Models;

namespace RestaurantReservation.Domain.CustomerAggregate.Handlers;

public class CreateCustomerHandler : ICommandHandler<CreateCustomer, CreateCustomerResult>
{
    private readonly IRepositoryBase<Customer, CustomerId> customerRepository;

    public CreateCustomerHandler(IRepositoryBase<Customer, CustomerId> customerRepository)
    {
        this.customerRepository = customerRepository;
    }

    public async Task<CreateCustomerResult> Handle(CreateCustomer command, CancellationToken cancellationToken)
    {
        var customer =
            await this.customerRepository.SingleOrDefaultAsync(x => command.Email == x.Email.Value, cancellationToken);
        if (customer != null) throw new CustomerAlreadyExistsException();

        var customerEntity = Customer.Create(
            new CustomerId(command.Id),
            firstName: command.FirstName,
            lastName: command.LastName,
            emailValue: command.Email);

        var newCustomer = await this.customerRepository.AddAsync(customerEntity, cancellationToken);
        await this.customerRepository.SaveChangesAsync(cancellationToken);

        return new CreateCustomerResult(newCustomer.Id);
    }
}
