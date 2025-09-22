﻿using InExTrack.Application.DTOs;
using InExTrack.Domain.Models;

namespace InExTrack.Application.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        public Task<IEnumerable<Transaction_>> GetTransactionsAsync(Guid userId, CancellationToken cancellationToken = default);
        public Task<Transaction_> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken = default);
        public Task<Transaction_> AddTransactionAsync(Transaction_ transaction, CancellationToken cancellationToken = default);
        public Task<Transaction_> UpdateTransactionAsync(Guid id, TransactionDto transactionDto, CancellationToken cancellationToken = default);
        public Task<bool> DeleteTransactionAsync(Guid id, CancellationToken cancellationToken = default);

    }
}
