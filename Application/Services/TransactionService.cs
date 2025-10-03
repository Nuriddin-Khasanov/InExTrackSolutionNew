using Application.DTOs;
using Application.DTOs.Responses;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Models;
using Mapster;

namespace Application.Services;

public class TransactionService(ITransactionRepository transactionRepository) : ITransactionService
{

    public async Task<ApiResponse<IEnumerable<TransactionDto>>> GetTransactionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var transactions = await transactionRepository.GetTransactionsAsync(userId, cancellationToken);

        if(!transactions.Any())
            return new ApiResponse<IEnumerable<TransactionDto>>(404, "Транзакции не найдены!");

        var transactionDtos = transactions.Adapt<IEnumerable<TransactionDto>>();

        return new ApiResponse<IEnumerable<TransactionDto>>(200, transactionDtos, "Транзакции успешно получены!");
    }

    public async Task<ApiResponse<TransactionDto>> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        var transaction = await transactionRepository.GetTransactionByIdAsync(transactionId, cancellationToken);

        if (transaction != null)
            return new ApiResponse<TransactionDto>(404, "Транзакция не найдена!");

        var transactionDtos = transaction.Adapt<TransactionDto>();

        return new ApiResponse<TransactionDto>(200, transactionDtos, "Транзакция успешно получено!");
    }

    public async Task<ApiResponse<TransactionDto>> AddTransactionAsync(Guid userId, TransactionDto transactionDto, CancellationToken cancellationToken = default)
    {
        var transactionAdapt = transactionDto.Adapt<Transaction_>();
        transactionAdapt.UserId = userId;

        var transaction = await transactionRepository.AddTransactionAsync(transactionAdapt, cancellationToken);

        var transactionDtos = transaction.Adapt<TransactionDto>();
        return new ApiResponse<TransactionDto>(201, transactionDtos, "Транзакция успешно добавлено!");
    }

    public async Task<ApiResponse<TransactionDto>> UpdateTransactionAsync(Guid id, TransactionDto transactionDto, CancellationToken cancellationToken = default)
    {
        var updatedTransaction = await transactionRepository.UpdateTransactionAsync(id, transactionDto, cancellationToken);

        var transactionDtos = updatedTransaction.Adapt<TransactionDto>();
        return new ApiResponse<TransactionDto>(200, transactionDtos, "Транзакция успешно изменено!");
    }

    public async Task<ApiResponse<bool>> DeleteTransactionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transactionDtos = await transactionRepository.DeleteTransactionAsync(id, cancellationToken);

        if (!transactionDtos)
            return new ApiResponse<bool>(404, "Транзакция не найдено!");

        return new ApiResponse<bool>(204, transactionDtos, "Транзакция успешно удалено!");
    }

    public async Task<ApiResponse<IEnumerable<TransactionDto>>> GetTransactionsByFilterAsync(Guid userId, TransactionFilterDto transactionFilterDto, CancellationToken cancellationToken)
    {
        var result = await transactionRepository.GetTransactionsByFilterAsync(userId, transactionFilterDto, cancellationToken);

        var transactionDtos = result.Adapt<IEnumerable<TransactionDto>>().ToList();
        
        return transactionDtos.Count != 0 
            ? new ApiResponse<IEnumerable<TransactionDto>>(200, transactionDtos, "Транзакция успешно получено!") 
            : new ApiResponse<IEnumerable<TransactionDto>>(404, "Транзакция не найдено!");
    }
}
