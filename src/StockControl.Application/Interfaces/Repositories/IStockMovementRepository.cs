using StockControl.Domain.Entities;

namespace StockControl.Application.Interfaces.Repositories;

public interface IStockMovementRepository
{
    Task<StockMovement?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<StockMovement>> ListAsync(CancellationToken cancellationToken = default);
    Task CreateAsync(StockMovement stockMovement, CancellationToken cancellationToken = default);
}
