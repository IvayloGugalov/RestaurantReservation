using MongoDB.Driver;
using RestaurantReservation.Core.CQRS;
using RestaurantReservation.Domain.RestaurantAggregate.Exceptions;
using RestaurantReservation.Domain.RestaurantAggregate.ValueObjects;
using RestaurantReservation.Infrastructure.Mongo.Data;

namespace RestaurantReservation.Api.Handlers.Restaurant;

public class CreateRestaurantHandler : ICommandHandler<CreateRestaurant, CreateRestaurantResult>
{
    private readonly AppMongoDbContext dbContext;

    public CreateRestaurantHandler(AppMongoDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CreateRestaurantResult> Handle(CreateRestaurant command, CancellationToken cancellationToken)
    {
        var restaurant =
            (await this.dbContext.Restaurants
                .FindAsync(Builders<Domain.RestaurantAggregate.Models.Restaurant>
                    .Filter
                    .Eq(x => x.Name, command.Name), cancellationToken: cancellationToken))
            .FirstOrDefault(cancellationToken: cancellationToken);
        if (restaurant != null) throw new RestaurantAlreadyExistsException();

        var restaurantEntity = Domain.RestaurantAggregate.Models.Restaurant.Create(
            new RestaurantId(command.Id),
            name: command.Name,
            phone: command.Phone,
            description: command.Description,
            url: command.Url,
            webSite: command.WebSite);

        restaurantEntity.SetWorkTime(command.WorkTime!);
        await this.dbContext.Restaurants.InsertOneAsync(restaurantEntity, new InsertOneOptions(), cancellationToken);

        return new CreateRestaurantResult(restaurantEntity.Id.Value);
    }
}
