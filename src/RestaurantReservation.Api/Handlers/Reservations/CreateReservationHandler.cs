using MongoDB.Driver;
using RestaurantReservation.Core.CQRS;
using RestaurantReservation.Domain.CustomerAggregate.Models;
using RestaurantReservation.Domain.ReservationAggregate.Exceptions;
using RestaurantReservation.Domain.ReservationAggregate.ValueObjects;
using RestaurantReservation.Domain.RestaurantAggregate.Models;
using RestaurantReservation.Infrastructure.Mongo.Data;

namespace RestaurantReservation.Api.Handlers.Reservations;

public class CreateReservationHandler : ICommandHandler<CreateReservation, CreateReservationResult>
{
    private readonly AppMongoDbContext dbContext;

    public CreateReservationHandler(AppMongoDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CreateReservationResult> Handle(CreateReservation command,
        CancellationToken ct)
    {
        var table =
            (await this.dbContext.Tables
                .FindAsync(Builders<Table>
                    .Filter
                    .Eq("_id", command.Id), cancellationToken: ct))
            .FirstOrDefault(cancellationToken: ct);

        if (table == null) throw new TableNotFoundException();

        var customer = (await this.dbContext.Customers
                .FindAsync(Builders<Customer>
                    .Filter
                    .Eq("_id", command.Id), cancellationToken: ct))
            .FirstOrDefault(cancellationToken: ct);

        var reservationEntity = table.AddReservation(
            new ReservationId(command.Id),
            customer ?? throw new Exception(""),
            command.ReservationDate,
            command.Occupants);

        // or replaceAsync
        await this.dbContext.Tables.UpdateOneAsync(
            x => x.Id.Value == table.Id,
            Builders<Table>.Update
                .Set(x => x.Reservations, table.Reservations),
            cancellationToken: ct);
        await this.dbContext.Reservations
            .InsertOneAsync(reservationEntity, new InsertOneOptions(), ct);

        return new CreateReservationResult(reservationEntity.Id.Value);
    }
}
