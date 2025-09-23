using Application.DTOs;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TransactionController(ITransactionService _transactionService) : ApiBaseController
{
    [HttpGet]
    public async Task<IActionResult> GetTransactions(CancellationToken cancellationToken)
    {
        if (UserUid is null || UserUid.Value == Guid.Empty)
            return BadRequest("UserUid не определён.");

        var transactions = await _transactionService.GetTransactionsAsync(UserUid, cancellationToken);
        return Ok(transactions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransactionById(Guid id, CancellationToken cancellationToken)
    {
        var transaction = await _transactionService.GetTransactionByIdAsync(id, cancellationToken);
        return Ok(transaction);
    }

    [HttpPost]
    public async Task<IActionResult> AddTransaction([FromBody] TransactionDto transactionDto, CancellationToken cancellationToken)
    {
        var createdTransaction = await _transactionService.AddTransactionAsync(transactionDto, cancellationToken);
        return Ok(createdTransaction);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTransaction(Guid id, [FromBody] TransactionDto transactionDto, CancellationToken cancellationToken)
    {
        var updatedTransaction = await _transactionService.UpdateTransactionAsync(id, transactionDto, cancellationToken);
        return Ok(updatedTransaction);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(Guid id, CancellationToken cancellationToken)
    {
        var result = await _transactionService.DeleteTransactionAsync(id, cancellationToken);

        return Ok(result);
    }

}
