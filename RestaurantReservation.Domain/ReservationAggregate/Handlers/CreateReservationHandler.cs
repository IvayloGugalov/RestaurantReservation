namespace RestaurantReservation.Domain.ReservationAggregate.Handlers;

public class CreateReservationHandler : ICommandHandler<CreateReservationEvent, CreateReservationResult>
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

    public async Task<CreateReservationResult> Handle(CreateReservationEvent request,
        CancellationToken cancellationToken)
    {
        // var table = await this.tableRepository.FirstOrDefaultAsync(x => request.TableId == x.Id, cancellationToken);
        // if (table == null) throw new TableNotFoundException();
        //
        // var reservationEntity = table.AddReservation(
        //     new ReservationId(request.Id),
        //     request.Customer,
        //     request.ReservationDate,
        //     request.Occupants);
        //
        // await this.tableRepository.UpdateAsync(table, cancellationToken);
        // await this.reservationRepository.AddAsync(reservationEntity, cancellationToken);
        //
        // return new CreateReservationResult(reservationEntity.Id);
        throw new NotImplementedException();
    }
}
