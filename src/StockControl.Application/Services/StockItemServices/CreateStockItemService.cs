using StockControl.Application.DTOs.Stocks;
using StockControl.Application.Exceptions;
using StockControl.Application.Interfaces;
using StockControl.Application.Interfaces.Repositories;
using StockControl.Domain.Entities;

namespace StockControl.Application.Services.StockItemServices;

public class CreateStockItemService
{
    private readonly IStockItemRepository _stockRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateStockItemService(IStockItemRepository stockRepository, IUnitOfWork unitOfWork)
    {
        _stockRepository = stockRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<StockResponseDto> ExecuteAsync(CreateStockDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.ProductId == Guid.Empty)
        {
            throw ValidationException.ForField(nameof(dto.ProductId), "ProductId is required.");
        }

        if (dto.QuantityAvailable < 0)
        {
            throw ValidationException.ForField(nameof(dto.QuantityAvailable), "QuantityAvailable must be zero or greater.");
        }

        var existingStock = await _stockRepository.FindByProductIdAsync(dto.ProductId, cancellationToken);
        if (existingStock is not null)
        {
            throw new ConflictException("StockItem already exists for this product.");
        }

        var stock = new StockItem
        {
            Id = Guid.NewGuid(),
            ProductId = dto.ProductId,
            QuantityAvailable = dto.QuantityAvailable,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _stockRepository.CreateAsync(stock, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
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
