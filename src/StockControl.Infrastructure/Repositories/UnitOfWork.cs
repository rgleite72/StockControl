using Microsoft.EntityFrameworkCore.Storage;
using StockControl.Application.Interfaces;
using StockControl.Infrastructure.Persistence;

namespace StockControl.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly StockDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(StockDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);

        if (_transaction is not null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
