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
                workTimeBuilder.WithOwner();

                workTimeBuilder.OwnsOne(wt => wt.Monday, b => ConfigureWorkingHours(b, "Monday"));
                workTimeBuilder.OwnsOne(wt => wt.Tuesday, b => ConfigureWorkingHours(b, "Tuesday"));
                workTimeBuilder.OwnsOne(wt => wt.Wednesday, b => ConfigureWorkingHours(b, "Wednesday"));
                workTimeBuilder.OwnsOne(wt => wt.Thursday, b => ConfigureWorkingHours(b, "Thursday"));
                workTimeBuilder.OwnsOne(wt => wt.Friday, b => ConfigureWorkingHours(b, "Friday"));
                workTimeBuilder.OwnsOne(wt => wt.Saturday, b => ConfigureWorkingHours(b, "Saturday"));
                workTimeBuilder.OwnsOne(wt => wt.Sunday, b => ConfigureWorkingHours(b, "Sunday"));
            });
        builder.Navigation(x=>x.WorkTime).IsRequired();
        // builder.Navigation(x => x.WorkTime).IsRequired();

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Phone).IsRequired();
        builder.Property(x => x.Url).IsRequired();
        builder.Property(x => x.WebSite).IsRequired();

        builder.HasMany(x => x.Reviews).WithOne(x => x.Restaurant);
        // builder.Navigation(x => x.Reviews).AutoInclude();
        builder.Metadata.FindNavigation(nameof(Restaurant.Reviews))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        // builder.Navigation(x => x.Reviews)
        //     .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(x => x.Tables).WithOne();
        // builder.Navigation(x => x.Tables).AutoInclude();
        builder.Metadata.FindNavigation(nameof(Restaurant.Tables))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        // builder.Navigation(x => x.Tables)
            // .UsePropertyAccessMode(PropertyAccessMode.Field);

    }

    private Action<OwnedNavigationBuilder<WorkTime, WorkingHours>> ConfigureWorkingHours()
    {
        throw new NotImplementedException();
    }

    private static void ConfigureWorkingHours(OwnedNavigationBuilder<WorkTime, WorkingHours> builder, string day)
    {
        builder.Property(wh => wh.OpeningTime).HasColumnName($"{day}_OpeningTime").IsRequired();
        builder.Property(wh => wh.ClosingTime).HasColumnName($"{day}_ClosingTime").IsRequired();
    }
}
