namespace RestaurantReservation.Core.Model;

public abstract class Entity<T> : IEntity<T> where T : IEquatable<T>
{
    public T Id { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
    public long? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public long? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
}
