namespace StockControl.Application.DTOs.Stocks;

public class StockResponseDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int QuantityAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
