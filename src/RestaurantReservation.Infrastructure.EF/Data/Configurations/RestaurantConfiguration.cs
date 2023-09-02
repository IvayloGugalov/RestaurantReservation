using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
            wt =>
            {
                ConfigureWorkingHours(wt.OwnsOne(t => t.Monday), nameof(WorkTime.Monday));
                ConfigureWorkingHours(wt.OwnsOne(t => t.Tuesday), nameof(WorkTime.Tuesday));
                ConfigureWorkingHours(wt.OwnsOne(t => t.Wednesday), nameof(WorkTime.Wednesday));
                ConfigureWorkingHours(wt.OwnsOne(t => t.Thursday), nameof(WorkTime.Thursday));
                ConfigureWorkingHours(wt.OwnsOne(t => t.Friday), nameof(WorkTime.Friday));
                ConfigureWorkingHours(wt.OwnsOne(t => t.Saturday), nameof(WorkTime.Saturday));
                ConfigureWorkingHours(wt.OwnsOne(t => t.Sunday), nameof(WorkTime.Sunday));
            });
        builder.Navigation(x => x.WorkTime).IsRequired();

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Phone).IsRequired();
        builder.Property(x => x.Url).IsRequired();
        builder.Property(x => x.WebSite).IsRequired();

        builder.HasMany(x => x.Reviews)
            .WithOne(x => x.Restaurant)
            .HasForeignKey(nameof(RestaurantId));
        builder.Metadata.FindNavigation(nameof(Restaurant.Reviews))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(x => x.Tables)
            .WithOne(x => x.Restaurant)
            .HasForeignKey(nameof(RestaurantId));
        builder.Metadata.FindNavigation(nameof(Restaurant.Tables))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureWorkingHours(OwnedNavigationBuilder<WorkTime, WorkingHours> builder, string day)
    {
        builder.Property(wh => wh.OpeningTime)
            .HasColumnName($"{day}_OpeningTime")
            .HasConversion(new TimeSpanToTicksConverter())
            .IsRequired();
        builder.Property(wh => wh.ClosingTime)
            .HasColumnName($"{day}_ClosingTime")
            .HasConversion(new TimeSpanToTicksConverter())
            .IsRequired();
    }
}
