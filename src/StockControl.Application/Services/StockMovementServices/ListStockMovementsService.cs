using StockControl.Application.DTOs.StockMovements;
using StockControl.Application.Interfaces.Repositories;

namespace StockControl.Application.Services.StockMovementServices;

public class ListStockMovementsService
{
    private readonly IStockMovementRepository _stockMovementRepository;

    public ListStockMovementsService(IStockMovementRepository stockMovementRepository)
    {
        _stockMovementRepository = stockMovementRepository;
    }

    public async Task<List<StockMovementResponseDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var movements = await _stockMovementRepository.ListAsync(cancellationToken);

        return movements.Select(stockMovement => new StockMovementResponseDto
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
        }).ToList();
    }
}
