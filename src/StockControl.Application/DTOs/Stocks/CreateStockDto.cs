namespace StockControl.Application.DTOs.Stocks;

public class CreateStockDto
{
    public Guid ProductId { get; set; }
    public int QuantityAvailable { get; set; }
}
