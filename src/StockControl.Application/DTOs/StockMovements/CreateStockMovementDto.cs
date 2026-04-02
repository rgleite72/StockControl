namespace StockControl.Application.DTOs.StockMovements;

public class CreateStockMovementDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string? TraceId { get; set; }
}
