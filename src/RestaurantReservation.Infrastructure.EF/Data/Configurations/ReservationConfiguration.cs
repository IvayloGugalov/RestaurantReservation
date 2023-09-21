// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// namespace RestaurantReservation.Infrastructure.EF.Data.Configurations;
//
// public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
// {
//     public void Configure(EntityTypeBuilder<Reservation> builder)
//     {
//         builder.ToTable(nameof(Reservation) + "s");
//
//         builder.HasKey(x => x.Id);
//         builder.Property(x => x.Id)
//             .ValueGeneratedNever()
//             .HasConversion(reservationId => reservationId.Value, dbId => new ReservationId(dbId));
//
//         builder.Property(x => x.ReservationDate)
//             .HasColumnType("date")
//             .IsRequired();
//         builder.Property(x => x.Occupants)
//             .IsRequired();
//         builder.Property(x => x.Status)
//             .HasConversion<string>()
//             .IsRequired();
//
//         builder.HasOne(x => x.Review)
//             .WithOne(x => x.Reservation)
//             .HasForeignKey<Review>(nameof(ReservationId))
//             .IsRequired(false);
//     }
// }
