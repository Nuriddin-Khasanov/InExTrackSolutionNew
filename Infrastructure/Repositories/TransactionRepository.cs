using Application.DTOs;
using Application.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.DataContext;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TransactionRepository(AppDbContext context) : ITransactionRepository
{

    public async Task<IEnumerable<Transaction_>> GetTransactionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.Transactions
            .Include(t => t.User)
            .Include(t => t.Category)
            .Where(t => t.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Transaction_> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        var transaction = await context.Transactions
            .Include(t => t.User)
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == transactionId, cancellationToken);

        return transaction ?? throw new KeyNotFoundException($"Transaction with ID {transactionId} not found.");
    }

    public async Task<Transaction_> AddTransactionAsync(Transaction_ transaction, CancellationToken cancellationToken = default)
    {
        await context.Transactions.AddAsync(transaction, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return transaction;
    }

    public async Task<Transaction_> UpdateTransactionAsync(Guid id, TransactionDto transactionDto, CancellationToken cancellationToken = default)
    {
        var existingTransaction = await context.Transactions
            .Include(t => t.User)
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"Transaction with ID {id} not found.");

        transactionDto.Adapt(existingTransaction);

        //_context.Transactions.Update(existingTransaction);

        await context.SaveChangesAsync(cancellationToken);
        return existingTransaction;

    }

    public async Task<bool> DeleteTransactionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await context.Transactions
            .Include(t => t.User)
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (transaction == null)
            return false;

        context.Transactions.Remove(transaction);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<Transaction_>> GetTransactionsByFilterAsync(Guid userId, TransactionFilterDto filter, CancellationToken cancellationToken = default)
    {
        var query = context.Transactions
            .Include(t => t.User)
            .Include(t => t.Category)
            .Where(t => t.UserId == userId)
            .AsQueryable();

        if (filter.CategoryId.HasValue)
            query = query.Where(t => t.CategoryId == filter.CategoryId.Value);

        if (filter.CategoryType.HasValue)
            query = query.Where(t => t.Category.Type == filter.CategoryType.Value);

        if (filter.MinAmount.HasValue)
            query = query.Where(t => t.Amount >= filter.MinAmount.Value);

        if (filter.MaxAmount.HasValue)
            query = query.Where(t => t.Amount <= filter.MaxAmount.Value);

        if (filter.DateFrom.HasValue)
            query = query.Where(t => t.CreatedDate >= filter.DateFrom.Value);

        if (filter.DateTo.HasValue)
            query = query.Where(t => t.CreatedDate <= filter.DateTo.Value);

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(t => t.Description!.Contains(filter.Search));

        return await query.ToListAsync(cancellationToken);
    }

}
