using StockControl.Application.DTOs.Stocks;
using StockControl.Application.Exceptions;
using StockControl.Application.Interfaces.Repositories;

namespace StockControl.Application.Services.StockItemServices;

public class GetStockItemByProductIdService
{
    private readonly IStockItemRepository _stockRepository;

    public GetStockItemByProductIdService(IStockItemRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task<StockResponseDto> ExecuteAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var stock = await _stockRepository.FindByProductIdAsync(productId, cancellationToken);
        if (stock is null)
        {
            throw new NotFoundException("StockItem not found for this product.");
        }

        return new StockResponseDto
        {
            Id = stock.Id,
            ProductId = stock.ProductId,
            QuantityAvailable = stock.QuantityAvailable,
            CreatedAt = stock.CreatedAt,
            UpdatedAt = stock.UpdatedAt
        };
    }
}
