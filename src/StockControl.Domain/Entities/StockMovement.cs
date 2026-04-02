using StockControl.Domain.Enums;

namespace StockControl.Domain.Entities;

public class StockMovement
{
    public Guid Id { get; set; }
    public Guid StockId { get; set; }
    public Guid ProductId { get; set; }
    public StockMovementType MovementType { get; set; }
    public int Quantity { get; set; }
    public int PreviousQuantity { get; set; }
    public int CurrentQuantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? TraceId { get; set; }
    public DateTime CreatedAt { get; set; }

    public StockItem? StockItem { get; set; }
}
