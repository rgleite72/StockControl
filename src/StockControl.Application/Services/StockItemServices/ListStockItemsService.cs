using StockControl.Application.DTOs.Stocks;
using StockControl.Application.Interfaces.Repositories;

namespace StockControl.Application.Services.StockItemServices;

public class ListStockItemsService
{
    private readonly IStockItemRepository _stockRepository;

    public ListStockItemsService(IStockItemRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task<List<StockResponseDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var stocks = await _stockRepository.ListAsync(cancellationToken);

        return stocks.Select(stock => new StockResponseDto
        {
            Id = stock.Id,
            ProductId = stock.ProductId,
            QuantityAvailable = stock.QuantityAvailable,
            CreatedAt = stock.CreatedAt,
            UpdatedAt = stock.UpdatedAt
        }).ToList();
    }
}
