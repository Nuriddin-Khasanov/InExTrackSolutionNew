using Application.DTOs;
using Application.DTOs.Responses;

namespace Application.Interfaces.Services;

public interface ITransactionService
{
    public Task<ApiResponse<IEnumerable<TransactionDto>>> GetTransactionsAsync(Guid? userId, CancellationToken cancellationToken = default);
    public Task<ApiResponse<TransactionDto>> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken = default);
    public Task<ApiResponse<TransactionDto>> AddTransactionAsync(TransactionDto transactionDto, CancellationToken cancellationToken = default);
    public Task<ApiResponse<TransactionDto>> UpdateTransactionAsync(Guid id, TransactionDto transactionDto, CancellationToken cancellationToken = default);
    public Task<ApiResponse<bool>> DeleteTransactionAsync(Guid id, CancellationToken cancellationToken = default);
}
