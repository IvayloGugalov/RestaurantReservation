using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Domain.CustomerAggregate;
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

        builder.Metadata.FindNavigation(nameof(Customer.FavouriteRestaurants))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}