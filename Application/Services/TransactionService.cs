using InExTrack.Application.DTOs;
using InExTrack.Application.DTOs.Responses;
using InExTrack.Application.Interfaces.Repositories;
using InExTrack.Application.Interfaces.Services;
using InExTrack.Domain.Models;
using Mapster;

namespace InExTrack.Application.Services
{
    public class TransactionService(ITransactionRepository _transactionRepository) : ITransactionService
    {

        public async Task<ApiResponse<IEnumerable<TransactionDto>>> GetTransactionsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var transactions = await _transactionRepository.GetTransactionsAsync(userId, cancellationToken);

            var transactionDtos = transactions.Adapt<IEnumerable<TransactionDto>>();

            return new ApiResponse<IEnumerable<TransactionDto>>(200, transactionDtos, "Транзакции успешно получены!");
        }

        public async Task<ApiResponse<TransactionDto>> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken = default)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId, cancellationToken);

            var transactionDtos = transaction.Adapt<TransactionDto>();

            return new ApiResponse<TransactionDto>(200, transactionDtos, "Транзакция успешно получено!");
        }

        public async Task<ApiResponse<TransactionDto>> AddTransactionAsync(TransactionDto transactionDto, CancellationToken cancellationToken = default)
        {
            var transactionAdapt = transactionDto.Adapt<Transaction_>();
            var transaction = await _transactionRepository.AddTransactionAsync(transactionAdapt, cancellationToken);

            var transactionDtos = transaction.Adapt<TransactionDto>();
            return new ApiResponse<TransactionDto>(201, transactionDtos, "Транзакция успешно добавлено!");
        }

        public async Task<ApiResponse<TransactionDto>> UpdateTransactionAsync(Guid id, TransactionDto transactionDto, CancellationToken cancellationToken = default)
        {
            var updatedTransaction = await _transactionRepository.UpdateTransactionAsync(id, transactionDto, cancellationToken);

            var transactionDtos = updatedTransaction.Adapt<TransactionDto>();
            return new ApiResponse<TransactionDto>(200, transactionDtos, "Транзакция успешно изменено!");
        }

        public async Task<ApiResponse<bool>> DeleteTransactionAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var transactionDtos = await _transactionRepository.DeleteTransactionAsync(id, cancellationToken);

            if (!transactionDtos)
                return new ApiResponse<bool>(404, "Транзакция не найдено!");

            return new ApiResponse<bool>(204, transactionDtos, "Транзакция успешно удалено!");
        }
    }
}
