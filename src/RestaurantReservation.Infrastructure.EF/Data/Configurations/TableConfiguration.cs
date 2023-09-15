// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// namespace RestaurantReservation.Infrastructure.EF.Data.Configurations;
//
// public class TableConfiguration : IEntityTypeConfiguration<Table>
// {
//     public void Configure(EntityTypeBuilder<Table> builder)
//     {
//         builder.ToTable(nameof(Table) + "s");
//
//         builder.HasKey(x => x.Id);
//         builder.Property(x => x.Id)
//             .ValueGeneratedNever()
//             .HasConversion(tableId => tableId.Value, dbId => new TableId(dbId));
//
//
//         builder.HasMany(x => x.Reservations).WithOne(x => x.Table);
//         builder.HasOne(x => x.Restaurant).WithMany(x => x.Tables);
//     }
// }
