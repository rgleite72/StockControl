using StockControl.Application.DTOs.StockMovements;
using StockControl.Application.Exceptions;
using StockControl.Application.Interfaces.Repositories;

namespace StockControl.Application.Services.StockMovementServices;

public class GetStockMovementByIdService
{
    private readonly IStockMovementRepository _stockMovementRepository;

    public GetStockMovementByIdService(IStockMovementRepository stockMovementRepository)
    {
        _stockMovementRepository = stockMovementRepository;
    }

    public async Task<StockMovementResponseDto> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var stockMovement = await _stockMovementRepository.FindByIdAsync(id, cancellationToken);
        if (stockMovement is null)
        {
            throw new NotFoundException("StockItem movement not found.");
        }

        return new StockMovementResponseDto
        {
            Id = stockMovement.Id,
            StockId = stockMovement.StockId,
            ProductId = stockMovement.ProductId,
            MovementType = stockMovement.MovementType.ToString(),
            Quantity = stockMovement.Quantity,
            PreviousQuantity = stockMovement.PreviousQuantity,
            CurrentQuantity = stockMovement.CurrentQuantity,
            Reason = stockMovement.Reason,
            TraceId = stockMovement.TraceId,
            CreatedAt = stockMovement.CreatedAt
        };
    }
}
