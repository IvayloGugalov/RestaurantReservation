using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RestaurantReservation.Infrastructure.EF.Data.Configurations;

public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.ToTable(nameof(Restaurant) + "s");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName(nameof(RestaurantId))
            .ValueGeneratedNever()
            .HasConversion(restaurantId => restaurantId.Value, dbId => new RestaurantId(dbId));

        builder.OwnsOne(
            x => x.WorkTime,
            workTimeBuilder =>
            {
                workTimeBuilder.OwnsOne(wt => wt.Monday);
                workTimeBuilder.OwnsOne(wt => wt.Tuesday);
                workTimeBuilder.OwnsOne(wt => wt.Wednesday);
                workTimeBuilder.OwnsOne(wt => wt.Thursday);
                workTimeBuilder.OwnsOne(wt => wt.Friday);
                workTimeBuilder.OwnsOne(wt => wt.Saturday);
                workTimeBuilder.OwnsOne(wt => wt.Sunday);
            });

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Phone).IsRequired();
        builder.Property(x => x.Url).IsRequired();
        builder.Property(x => x.WebSite).IsRequired();

        builder.HasMany(x => x.Reviews)
            .WithOne()
            .HasForeignKey(nameof(RestaurantId));
        builder.Metadata.FindNavigation(nameof(Restaurant.Reviews))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}