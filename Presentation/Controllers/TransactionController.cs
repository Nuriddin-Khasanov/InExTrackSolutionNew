using Application.DTOs;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TransactionController(ITransactionService transactionService) : ApiBaseController
{
    [HttpGet]
    public async Task<IActionResult> GetTransactions(CancellationToken cancellationToken)
    {
        var transactions = await transactionService.GetTransactionsAsync(GetUserId(), cancellationToken);

        return Ok(transactions);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTransactionById(Guid id, CancellationToken cancellationToken)
    {
        var transaction = await transactionService.GetTransactionByIdAsync(id, cancellationToken);

        return Ok(transaction);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetTransactionsByFilter([FromQuery] TransactionFilterDto transactionFilterDto, CancellationToken cancellationToken)
    {
        var result = await transactionService.GetTransactionsByFilterAsync(GetUserId(), transactionFilterDto, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddTransaction([FromBody] TransactionDto transactionDto, CancellationToken cancellationToken)
    {
        var createdTransaction = await transactionService.AddTransactionAsync(GetUserId(), transactionDto, cancellationToken);

        return Ok(createdTransaction);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTransaction(Guid id, [FromBody] TransactionDto transactionDto, CancellationToken cancellationToken)
    {
        var updatedTransaction = await transactionService.UpdateTransactionAsync(id, transactionDto, cancellationToken);

        return Ok(updatedTransaction);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTransaction(Guid id, CancellationToken cancellationToken)
    {
        var result = await transactionService.DeleteTransactionAsync(id, cancellationToken);

        return Ok(result);
    }

}
