using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockControl.Domain.Entities;

namespace StockControl.Infrastructure.Configuration;

public class StockItemConfiguration : IEntityTypeConfiguration<StockItem>
{
    public void Configure(EntityTypeBuilder<StockItem> builder)
    {
        builder.ToTable("stocks");

        builder.HasKey(stock => stock.Id);

        builder.Property(stock => stock.ProductId)
            .IsRequired();

        builder.Property(stock => stock.QuantityAvailable)
            .IsRequired();

        builder.Property(stock => stock.CreatedAt)
            .IsRequired();

        builder.Property(stock => stock.UpdatedAt)
            .IsRequired();

        builder.HasIndex(stock => stock.ProductId)
            .IsUnique();
    }
}
