using StockControl.Application.DTOs.Stocks;
using StockControl.Application.Exceptions;
using StockControl.Application.Interfaces.Repositories;

namespace StockControl.Application.Services.StockItemServices;

public class GetStockItemByIdService
{
    private readonly IStockItemRepository _stockRepository;

    public GetStockItemByIdService(IStockItemRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task<StockResponseDto> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var stock = await _stockRepository.FindByIdAsync(id, cancellationToken);
        if (stock is null)
        {
            throw new NotFoundException("StockItem not found.");
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
