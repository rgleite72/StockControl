namespace StockControl.Application.DTOs.StockMovements;

public class StockMovementResponseDto
{
    public Guid Id { get; set; }
    public Guid StockId { get; set; }
    public Guid ProductId { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int PreviousQuantity { get; set; }
    public int CurrentQuantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? TraceId { get; set; }
    public DateTime CreatedAt { get; set; }
}
