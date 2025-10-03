using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces.Services;

public interface ITransactionService
{
    public Task<ApiResponse<IEnumerable<TransactionResponseDto>>> GetTransactionsAsync(Guid userId, CancellationToken cancellationToken = default);
    public Task<ApiResponse<TransactionResponseDto>> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken = default);
    public Task<ApiResponse<TransactionResponseDto>> AddTransactionAsync(Guid userId, TransactionRequestDto transactionDto, CancellationToken cancellationToken = default);
    public Task<ApiResponse<TransactionResponseDto>> UpdateTransactionAsync(Guid id, TransactionRequestDto transactionDto, CancellationToken cancellationToken = default);
    public Task<ApiResponse<bool>> DeleteTransactionAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<ApiResponse<IEnumerable<TransactionResponseDto>>> GetTransactionsByFilterAsync(Guid userId, TransactionFilterDto transactionFilterDto, CancellationToken cancellationToken);
}
