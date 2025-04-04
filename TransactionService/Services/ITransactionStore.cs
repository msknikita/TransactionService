namespace TransactionService.Services;

public interface ITransactionStore
{
    Task<CreateTransaction.Response> CreateAsync(Transaction transaction, CancellationToken cts);
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cts);
}