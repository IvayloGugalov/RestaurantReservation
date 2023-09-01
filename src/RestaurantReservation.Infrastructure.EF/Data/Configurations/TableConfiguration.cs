using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RestaurantReservation.Infrastructure.EF.Data.Configurations;

public class TableConfiguration : IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {
        builder.ToTable(nameof(Table) + "s");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName(nameof(TableId))
            .ValueGeneratedNever()
            .HasConversion(tableId => tableId.Value, dbId => new TableId(dbId));

        builder.HasMany(x => x.Reservations).WithOne();
        // builder.Navigation(x => x.Reservations).AutoInclude();
        builder.Metadata.FindNavigation(nameof(Table.Reservations))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        // builder.Navigation(x => x.Reservations)
        //     .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne(x => x.Restaurant)
            .WithMany(x => x.Tables)
            .HasForeignKey(nameof(RestaurantId))
            .IsRequired();
    }
}
