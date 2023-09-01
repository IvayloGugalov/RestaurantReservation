using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RestaurantReservation.Infrastructure.EF.Data.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable(nameof(Reservation) + "s");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName(nameof(ReservationId))
            .ValueGeneratedNever()
            .HasConversion(reservationId => reservationId.Value, dbId => new ReservationId(dbId));

        builder.Property(x => x.ReservationDate)
            .HasColumnType("date")
            .IsRequired();
        builder.Property(x => x.Occupants)
            .IsRequired();
        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.RestaurantId)
            .HasColumnName(nameof(RestaurantId))
            .HasConversion(restaurantId => restaurantId.Value, dbId => new RestaurantId(dbId))
            .IsRequired();

        builder.Property(x => x.CustomerId)
            .HasColumnName(nameof(CustomerId))
            .HasConversion(customerId => customerId.Value, dbId => new CustomerId(dbId))
            .IsRequired();

        builder.Property(x => x.TableId)
            .HasColumnName(nameof(TableId))
            .HasConversion(tableId => tableId.Value, dbId => new TableId(dbId))
            .IsRequired();

        builder.Property(x => x.ReviewId)
            .HasColumnName(nameof(ReviewId))
            .HasConversion(reviewId => reviewId != null ? reviewId.Value : Guid.Empty , dbId => new ReviewId(dbId))
            .IsRequired(false);
    }
}
