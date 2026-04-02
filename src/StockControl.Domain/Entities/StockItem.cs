namespace StockControl.Domain.Entities;

public class StockItem
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int QuantityAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<StockMovement> Movements { get; set; } = new List<StockMovement>();
}
