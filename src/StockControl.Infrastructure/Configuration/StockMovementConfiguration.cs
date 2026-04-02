using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockControl.Domain.Entities;

namespace StockControl.Infrastructure.Configuration;

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("stock_movements");

        builder.HasKey(stockMovement => stockMovement.Id);

        builder.Property(stockMovement => stockMovement.ProductId)
            .IsRequired();

        builder.Property(stockMovement => stockMovement.MovementType)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(stockMovement => stockMovement.Quantity)
            .IsRequired();

        builder.Property(stockMovement => stockMovement.PreviousQuantity)
            .IsRequired();

        builder.Property(stockMovement => stockMovement.CurrentQuantity)
            .IsRequired();

        builder.Property(stockMovement => stockMovement.Reason)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(stockMovement => stockMovement.TraceId)
            .HasMaxLength(100);

        builder.Property(stockMovement => stockMovement.CreatedAt)
            .IsRequired();

        builder.HasOne(stockMovement => stockMovement.StockItem)
            .WithMany(stock => stock.Movements)
            .HasForeignKey(stockMovement => stockMovement.StockId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
