using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RestaurantReservation.Infrastructure.EF.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable(nameof(Review) + "s");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName(nameof(ReviewId))
            .ValueGeneratedNever()
            .HasConversion(reviewId => reviewId.Value, dbId => new ReviewId(dbId));

        builder.Property(x => x.ReservationId)
            .HasColumnName(nameof(ReservationId))
            .HasConversion(reservationId => reservationId != null ? reservationId.Value : Guid.Empty, dbId => new ReservationId(dbId))
            .IsRequired(false);

        builder.Property(x => x.CustomerId)
            .HasColumnName(nameof(CustomerId))
            .HasConversion(customerId => customerId.Value, dbId => new CustomerId(dbId))
            .IsRequired();

        builder.Property(x => x.Comment).IsRequired(false);
        builder.Property(x => x.CustomerName).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.OwnsOne(
            x => x.Rating,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Rating))
                    .IsRequired();
            });
    }
}
