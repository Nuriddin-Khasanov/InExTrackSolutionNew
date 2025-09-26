using Application.DTOs;
using Application.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.DataContext;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TransactionRepository(AppDBContext _context) : ITransactionRepository
{

    public async Task<IEnumerable<Transaction_>> GetTransactionsAsync(Guid? userId, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Where(t => t.UserCategories.Any(uc => uc.UserId == userId && uc.IsActive))
            .ToListAsync(cancellationToken);

        //return await _context.Transactions
        //    .Where(t => t.UserCategory!.UserId == userId && t.UserCategory.IsActive)
        //    .ToListAsync(cancellationToken);

        //return await _context.Categories
        //   .Include(c => c.Image)
        //   .Where(c => c.UserCategories.Any(uc => uc.UserId == userId && uc.IsActive))
        //   .ToListAsync(cancellationToken);
    }

    public async Task<Transaction_> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId, cancellationToken);

        if (transaction == null)
            throw new KeyNotFoundException($"Transaction with ID {transactionId} not found.");

        return transaction;
    }

    public async Task<Transaction_> AddTransactionAsync(Transaction_ transaction, CancellationToken cancellationToken = default)
    {
        await _context.Transactions.AddAsync(transaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return transaction;
    }

    public async Task<Transaction_> UpdateTransactionAsync(Guid id, TransactionDto transactionDto, CancellationToken cancellationToken = default)
    {
        var existingTransaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (existingTransaction == null)
            throw new KeyNotFoundException($"Transaction with ID {id} not found.");

        transactionDto.Adapt(existingTransaction);

        //_context.Transactions.Update(existingTransaction);

        await _context.SaveChangesAsync(cancellationToken);
        return existingTransaction;

    }

    public async Task<bool> DeleteTransactionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (transaction == null)
            return false;

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

}
