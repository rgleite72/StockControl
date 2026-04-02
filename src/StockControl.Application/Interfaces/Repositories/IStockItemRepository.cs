using StockControl.Domain.Entities;

namespace StockControl.Application.Interfaces.Repositories;

public interface IStockItemRepository
{
    Task<StockItem?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<StockItem?> FindByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<List<StockItem>> ListAsync(CancellationToken cancellationToken = default);
    Task CreateAsync(StockItem stock, CancellationToken cancellationToken = default);
    void Update(StockItem stock);
}
