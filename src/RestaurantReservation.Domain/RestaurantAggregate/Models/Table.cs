namespace RestaurantReservation.Domain.RestaurantAggregate.Models;

public class Table : AggregateRoot<TableId>
{
    public string Number { get; private init; } = null!;
    public ushort Capacity { get; private init; }
    public Restaurant Restaurant { get; private init; } = null!;

    private readonly List<Reservation> reservations;
    public IReadOnlyCollection<Reservation> Reservations => this.reservations.AsReadOnly();

    private Table()
    {
        this.reservations = new List<Reservation>();
    }

    public static Table Create(
        TableId id,
        string number,
        ushort capacity,
        Restaurant restaurant)
    {
        var table = new Table
        {
            Id = id,
            Number = number,
            Capacity = capacity,
            Restaurant = restaurant
        };

        return table;
    }
}