// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
//
// namespace RestaurantReservation.Infrastructure.EF.Data.Configurations;
//
// public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
// {
//     public void Configure(EntityTypeBuilder<Restaurant> builder)
//     {
//         builder.ToTable(nameof(Restaurant) + "s");
//
//         builder.HasKey(x => x.Id);
//         builder.Property(x => x.Id)
//             .ValueGeneratedNever()
//             .HasConversion(restaurantId => restaurantId.Value, dbId => new RestaurantId(dbId));
//
//         // builder.HasOne(x => x.WorkTime)
//         //     .WithOne(x => x.Restaurant)
//         //     .HasPrincipalKey<WorkTime>(x => x.Id)
//         //     .IsRequired();
//         // builder.OwnsOne(
//         //     x => x.WorkTime,
//         //     wt =>
//         //     {
//         //         // wt.ToTable("restaurants_working_hours");
//         //         wt.Property(x => x.Id).ValueGeneratedOnAdd();
//         //         wt.HasKey(x => x.Id);
//         //
//         //         wt.WithOwner(t => t.Restaurant).HasForeignKey(x => x.RestaurantId);
//         //
//         //         ConfigureWorkingHours(wt.OwnsOne(t => t.Monday), nameof(WorkTime.Monday));
//         //         ConfigureWorkingHours(wt.OwnsOne(t => t.Tuesday), nameof(WorkTime.Tuesday));
//         //         ConfigureWorkingHours(wt.OwnsOne(t => t.Wednesday), nameof(WorkTime.Wednesday));
//         //         ConfigureWorkingHours(wt.OwnsOne(t => t.Thursday), nameof(WorkTime.Thursday));
//         //         ConfigureWorkingHours(wt.OwnsOne(t => t.Friday), nameof(WorkTime.Friday));
//         //         ConfigureWorkingHours(wt.OwnsOne(t => t.Saturday), nameof(WorkTime.Saturday));
//         //         ConfigureWorkingHours(wt.OwnsOne(t => t.Sunday), nameof(WorkTime.Sunday));
//         //
//         //         wt.Navigation(x => x.Monday).IsRequired(false);
//         //         wt.Navigation(x => x.Tuesday).IsRequired(false);
//         //         wt.Navigation(x => x.Wednesday).IsRequired(false);
//         //         wt.Navigation(x => x.Thursday).IsRequired(false);
//         //         wt.Navigation(x => x.Friday).IsRequired(false);
//         //         wt.Navigation(x => x.Saturday).IsRequired(false);
//         //         wt.Navigation(x => x.Sunday).IsRequired(false);
//         //
//         //     }).ToTable("RestaurantsWorkTime");
//         builder.Navigation(x => x.WorkTime).IsRequired();
//
//         builder.Property(x => x.Name).IsRequired();
//         builder.Property(x => x.Description).IsRequired();
//         builder.Property(x => x.Phone).IsRequired();
//         builder.Property(x => x.Url).IsRequired(false);
//         builder.Property(x => x.WebSite).IsRequired(false);
//
//         builder.HasMany(x => x.Reviews)
//             .WithOne(x => x.Restaurant);
//         builder.Metadata.FindNavigation(nameof(Restaurant.Reviews))
//             ?.SetPropertyAccessMode(PropertyAccessMode.Field);
//
//         builder.HasMany(x => x.Tables)
//             .WithOne(x => x.Restaurant);
//         builder.Metadata.FindNavigation(nameof(Restaurant.Tables))
//             ?.SetPropertyAccessMode(PropertyAccessMode.Field);
//     }
//
//     // private static void ConfigureWorkingHours(OwnedNavigationBuilder<WorkTime, WorkingHours> builder, string day)
//     // {
//     //     builder.Property(wh => wh.OpeningTime)
//     //         .HasColumnName($"{day.ToLower()}_opening_time")
//     //         .HasConversion(new TimeSpanToTicksConverter())
//     //         .IsRequired(false);
//     //     builder.Property(wh => wh.ClosingTime)
//     //         .HasColumnName($"{day.ToLower()}_closing_time")
//     //         .HasConversion(new TimeSpanToTicksConverter())
//     //         .IsRequired(false);
//     // }
// }
