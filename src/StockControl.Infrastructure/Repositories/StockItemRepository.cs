using Microsoft.EntityFrameworkCore;
using StockControl.Application.Interfaces.Repositories;
using StockControl.Domain.Entities;
using StockControl.Infrastructure.Persistence;

namespace StockControl.Infrastructure.Repositories;

public class StockItemRepository : IStockItemRepository
{
    private readonly StockDbContext _context;

    public StockItemRepository(StockDbContext context)
    {
        _context = context;
    }

    public async Task<StockItem?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Stocks
            .FirstOrDefaultAsync(stock => stock.Id == id, cancellationToken);
    }

    public async Task<StockItem?> FindByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _context.Stocks
            .FirstOrDefaultAsync(stock => stock.ProductId == productId, cancellationToken);
    }

    public async Task<List<StockItem>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Stocks
            .OrderBy(stock => stock.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(StockItem stock, CancellationToken cancellationToken = default)
    {
        await _context.Stocks.AddAsync(stock, cancellationToken);
    }

    public void Update(StockItem stock)
    {
        _context.Stocks.Update(stock);
    }
}
