namespace RestaurantReservation.Domain.ReservationAggregate.Handlers;

public class CreateReservationHandler : ICommandHandler<CreateReservation, CreateReservationResult>
{
    private readonly IRepositoryBase<Table, TableId> tableRepository;
    private readonly IRepositoryBase<Reservation, ReservationId> reservationRepository;

    public CreateReservationHandler(
        IRepositoryBase<Table, TableId> tableRepository,
        IRepositoryBase<Reservation, ReservationId> reservationRepository)
    {
        this.tableRepository = tableRepository;
        this.reservationRepository = reservationRepository;
    }

    public async Task<CreateReservationResult> Handle(CreateReservation command,
        CancellationToken cancellationToken)
    {
        var table = await this.tableRepository.SingleOrDefaultAsync(x => command.TableId == x.Id, cancellationToken);
        if (table == null) throw new TableNotFoundException();

        var reservationEntity = table.AddReservation(
            new ReservationId(command.Id),
            command.CustomerId,
            command.ReservationDate,
            command.Occupants);

        await this.tableRepository.UpdateAsync(table, cancellationToken);
        await this.reservationRepository.AddAsync(reservationEntity, cancellationToken);

        return new CreateReservationResult(reservationEntity.Id.Value);
    }
}
