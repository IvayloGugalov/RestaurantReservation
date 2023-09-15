// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// using RestaurantReservation.Domain.CustomerAggregate.Models;
//
// namespace RestaurantReservation.Infrastructure.EF.Data.Configurations;
//
// public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
// {
//     public void Configure(EntityTypeBuilder<Customer> builder)
//     {
//         builder.ToTable(nameof(Customer) + "s");
//
//         builder.HasKey(x => x.Id);
//         builder.Property(x => x.Id)
//             .ValueGeneratedNever()
//             .HasConversion(customerId => customerId.Value, dbId => new CustomerId(dbId));
//
//         builder.OwnsOne(
//             x => x.FullName,
//             nameBuilder =>
//             {
//                 nameBuilder.Property(p => p.FirstName)
//                     .HasColumnName("first_name")
//                     .HasMaxLength(100)
//                     .IsRequired();
//
//                 nameBuilder.Property(p => p.LastName)
//                     .HasColumnName("last_name")
//                     .HasMaxLength(100)
//                     .IsRequired();
//             });
//
//         builder.OwnsOne(
//             x => x.Email,
//             a =>
//             {
//                 a.Property(p => p.Value)
//                     .HasColumnName(nameof(Email).ToLower())
//                     .HasMaxLength(150)
//                     .IsRequired();
//             });
//
//         builder.HasMany(x => x.FavouriteRestaurants)
//             .WithMany()
//             .UsingEntity(
//                 "CustomersFavouriteRestaurants",
//                 l => l.HasOne(typeof(Restaurant)).WithMany().OnDelete(DeleteBehavior.NoAction),
//                 r => r.HasOne(typeof(Customer)).WithMany().OnDelete(DeleteBehavior.NoAction));
//
//         builder.HasMany(x => x.Reservations)
//             .WithOne(x => x.Customer)
//             .OnDelete(DeleteBehavior.NoAction);
//     }
// }
