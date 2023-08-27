namespace RestaurantReservation.Domain.RestaurantAggregate.Handlers;

public class AddTableHandler : ICommandHandler<AddTable, AddTableResult>
{
    private readonly IRepositoryBase<Restaurant, RestaurantId> restaurantRepository;

    public AddTableHandler(IRepositoryBase<Restaurant, RestaurantId> restaurantRepository)
    {
        this.restaurantRepository = restaurantRepository;
    }

    public async Task<AddTableResult> Handle(AddTable command, CancellationToken cancellationToken)
    {
        var restaurant =
            await this.restaurantRepository.GetByIdAsync(command.RestaurantId, cancellationToken);
        if (restaurant == null) throw new NullReferenceException("Restaurant is not found");

        var table = restaurant.AddTable(new TableId(command.Id), command.Number, command.Capacity);
        await this.restaurantRepository.SaveChangesAsync(cancellationToken);

        return new AddTableResult(table.Id);
    }
}
