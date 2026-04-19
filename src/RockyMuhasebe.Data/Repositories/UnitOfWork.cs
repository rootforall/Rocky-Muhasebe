using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RockyMuhasebe.Data.Context;
using RockyMuhasebe.Domain.Interfaces;

namespace RockyMuhasebe.Data.Repositories;

/// <summary>
/// Unit of Work implementasyonu - İşlem yönetimi
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly RockyDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(RockyDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
