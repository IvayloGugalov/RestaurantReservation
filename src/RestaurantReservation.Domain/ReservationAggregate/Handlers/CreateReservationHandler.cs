

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
        var table = await this.tableRepository.GetByIdAsync(command.TableId, ct);
        if (table == null) throw new TableNotFoundException();

        var customer = await this.customerRepository.GetByIdAsync(command.CustomerId, ct);

        var reservationEntity = table.AddReservation(
            new ReservationId(command.Id),
            customer ?? throw new Exception(""),
            command.ReservationDate,
            command.Occupants);

        this.tableRepository.UpdateAsync(table, ct);
        this.reservationRepository.AddAsync(reservationEntity, ct);

        return new CreateReservationResult(reservationEntity.Id.Value);
    }
}
