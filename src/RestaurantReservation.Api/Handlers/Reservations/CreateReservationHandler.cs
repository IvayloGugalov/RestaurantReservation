using RestaurantReservation.Core.CQRS;
using RestaurantReservation.Domain.CustomerAggregate.ValueObjects;
using RestaurantReservation.Domain.ReservationAggregate.Exceptions;
using RestaurantReservation.Domain.ReservationAggregate.Models;
using RestaurantReservation.Domain.ReservationAggregate.ValueObjects;
using RestaurantReservation.Domain.RestaurantAggregate.Models;
using RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;
using RestaurantReservation.Infrastructure.Mongo.Repositories;

namespace RestaurantReservation.Api.Handlers.Reservations;

public class CreateReservationHandler : ICommandHandler<CreateReservation, CreateReservationResult>
{
    private readonly IMongoRepository<Table, TableId> tableRepository;
    private readonly IMongoRepository<Reservation, ReservationId> reservationRepository;
    private readonly IMongoRepository<Domain.CustomerAggregate.Models.Customer, CustomerId> customerRepository;

    public CreateReservationHandler(
        IMongoRepository<Table, TableId> tableRepository,
        IMongoRepository<Reservation, ReservationId> reservationRepository,
        IMongoRepository<Domain.CustomerAggregate.Models.Customer, CustomerId> customerRepository)
    {
        this.tableRepository = tableRepository;
        this.reservationRepository = reservationRepository;
        this.customerRepository = customerRepository;
    }

    public async Task<CreateReservationResult> Handle(CreateReservation command,
        CancellationToken cancellationToken)
    {
        var table = await this.tableRepository.SingleOrDefaultAsync(x => command.TableId == x.Id, cancellationToken);
        if (table == null) throw new TableNotFoundException();

        var customer = await this.customerRepository.SingleOrDefaultAsync(x => command.CustomerId == x.Id, cancellationToken);

        var reservationEntity = table.AddReservation(
            new ReservationId(command.Id),
            customer ?? throw new Exception(""),
            command.ReservationDate,
            command.Occupants);

        await this.tableRepository.UpdateAsync(table, cancellationToken);
        await this.reservationRepository.AddAsync(reservationEntity, cancellationToken);

        return new CreateReservationResult(reservationEntity.Id.Value);
    }
}
