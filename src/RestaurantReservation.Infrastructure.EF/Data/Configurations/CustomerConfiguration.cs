using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RestaurantReservation.Domain.CustomerAggregate.Models;

namespace RestaurantReservation.Infrastructure.EF.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable(nameof(Customer) + "s");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName(nameof(CustomerId))
            .ValueGeneratedNever()
            .HasConversion(customerId => customerId.Value, dbId => new CustomerId(dbId));

        builder.OwnsOne(
            x => x.FullName,
            nameBuilder =>
            {
                nameBuilder.Property(p => p.FirstName)
                    .HasColumnName(nameof(CustomerName.FirstName))
                    .HasMaxLength(100)
                    .IsRequired();

                nameBuilder.Property(p => p.LastName)
                    .HasColumnName(nameof(CustomerName.LastName))
                    .HasMaxLength(100)
                    .IsRequired();
            });

        builder.OwnsOne(
            x => x.Email,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Email))
                    .HasMaxLength(150)
                    .IsRequired();
            });

        builder.OwnsMany(
            x => x.Reservations,
            reservationBuilder =>
            {
                reservationBuilder.WithOwner();
                reservationBuilder.ToTable("CustomerReservationIds");
                reservationBuilder.HasKey("Id");
                reservationBuilder.Property(x => x.Value)
                    .ValueGeneratedNever()
                    .HasColumnName("CustomerReservationId");
            });
        builder.Metadata.FindNavigation(nameof(Customer.Reservations))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(
            x => x.FavouriteRestaurants,
            restaurantBuilder =>
            {
                restaurantBuilder.WithOwner();
                restaurantBuilder.ToTable("CustomerFavouriteRestaurantIds");
                restaurantBuilder.HasKey("Id");
                restaurantBuilder.Property(x => x.Value)
                    .ValueGeneratedNever()
                    .HasColumnName("CustomerFavouriteRestaurantId");
            });
        builder.Metadata.FindNavigation(nameof(Customer.FavouriteRestaurants))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);

        // builder.HasMany(x => x.Reservations)
        //     .WithOne()
        //     .HasForeignKey(nameof(ReservationId))
        //     .HasPrincipalKey(x => x.Id);
        // builder.Navigation(x => x.Reservations)
        //     .UsePropertyAccessMode(PropertyAccessMode.Field);

        // .HasForeignKey(nameof(ReservationId));
        // builder.Navigation(x => x.Reservations)
        //     .AutoInclude()
        //     .UsePropertyAccessMode(PropertyAccessMode.Field);
        // builder.Navigation(x => x.Reservations)
        //     .UsePropertyAccessMode(PropertyAccessMode.Field);

        // builder.HasMany(x => x.FavouriteRestaurants)
        //     .WithMany()
        //     .UsingEntity<UserRestaurant>(
        //         l =>
        //         {
        //             l.Property(x => x.CustomerId).HasConversion(customerId => customerId.Value, dbId => new CustomerId(dbId));
        //             return l.HasOne<Restaurant>().WithMany().HasForeignKey(e => e.RestaurantId).HasPrincipalKey(x => x.Id);
        //         },
        //         r =>
        //         {
        //             r.Property(x => x.RestaurantId).HasConversion(restaurantId => restaurantId.Value, dbId => new RestaurantId(dbId));
        //             return  r.HasOne<Customer>().WithMany().HasForeignKey(e => e.CustomerId).HasPrincipalKey(nameof(CustomerId));
        //         });
        // builder.Navigation(x => x.FavouriteRestaurants)
        //     .UsePropertyAccessMode(PropertyAccessMode.Field);

        // builder.Navigation(x => x.FavouriteRestaurants).AutoInclude(false);
        // builder.Navigation(x => x.FavouriteRestaurants)
        //     .UsePropertyAccessMode(PropertyAccessMode.Field);

    }
}
