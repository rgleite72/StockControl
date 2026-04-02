using Microsoft.EntityFrameworkCore;
using StockControl.Application.Interfaces.Repositories;
using StockControl.Domain.Entities;
using StockControl.Infrastructure.Persistence;

namespace StockControl.Infrastructure.Repositories;

public class StockMovementRepository : IStockMovementRepository
{
    private readonly StockDbContext _context;

    public StockMovementRepository(StockDbContext context)
    {
        _context = context;
    }

    public async Task<StockMovement?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.StockMovements
            .FirstOrDefaultAsync(stockMovement => stockMovement.Id == id, cancellationToken);
    }

    public async Task<List<StockMovement>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.StockMovements
            .OrderByDescending(stockMovement => stockMovement.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(StockMovement stockMovement, CancellationToken cancellationToken = default)
    {
        await _context.StockMovements.AddAsync(stockMovement, cancellationToken);
    }
}
