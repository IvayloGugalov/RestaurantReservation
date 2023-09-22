using RestaurantReservation.Core.Mongo;

namespace RestaurantReservation.Domain.ReservationAggregate.Handlers;

public class CreateReservationHandler : ICommandHandler<CreateReservation, CreateReservationResult>
{
    private readonly IMongoRepository<Reservation, ReservationId> reservationRepository;
    private readonly IMongoRepository<Table, TableId> tableRepository;
    private readonly IMongoRepository<Customer, CustomerId> customerRepository;

    public CreateReservationHandler(
        IMongoRepository<Reservation, ReservationId> reservationRepository,
        IMongoRepository<Table, TableId> tableRepository,
        IMongoRepository<Customer, CustomerId> customerRepository)
    {
        this.reservationRepository = reservationRepository;
        this.tableRepository = tableRepository;
        this.customerRepository = customerRepository;
    }

    public async Task<CreateReservationResult> Handle(CreateReservation command,
        CancellationToken ct)
    {
        var table = await this.tableRepository.GetByIdAsync(new TableId(command.TableId), ct);
        if (table == null) throw new TableNotFoundException();

        var customer = await this.customerRepository.GetByIdAsync(new CustomerId(command.CustomerId), ct);

        var reservationEntity = table.AddReservation(
            new ReservationId(command.Id),
            customer ?? throw new CustomerNotFoundException(),
            command.ReservationDate,
            command.Occupants);

        await this.tableRepository.UpdateAsync(table, ct);
        await this.reservationRepository.AddAsync(reservationEntity, ct);

        return new CreateReservationResult(reservationEntity.Id.Value);
    }
}
