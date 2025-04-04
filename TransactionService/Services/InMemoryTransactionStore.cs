namespace TransactionService.Services;

public class InMemoryTransactionStore : ITransactionStore
{
    private readonly ConcurrentDictionary<Guid, (Transaction transaction, DateTime insertTime)> _storage = new();
    private readonly object _lock = new();

    public Task<CreateTransaction.Response> CreateAsync(Transaction transaction, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        if (_storage.TryGetValue(transaction.Id, out var existing))
            return Task.FromResult(new CreateTransaction.Response(existing.insertTime));

        var timeNow = DateTime.UtcNow;

        lock (_lock)
        {
            if (_storage.Count >= 100)
            {
                var oldest = _storage.OrderBy(x => x.Value.insertTime).First().Key;
                _storage.TryRemove(oldest, out _);
            }

            _storage[transaction.Id] = (transaction, timeNow);
        }

        return Task.FromResult(new CreateTransaction.Response(timeNow));
    }

    public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        return Task.FromResult(_storage.TryGetValue(id, out var value) ? value.transaction : null);
    }
}