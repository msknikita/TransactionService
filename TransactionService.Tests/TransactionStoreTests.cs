using TransactionService.Model.Transactions;
using TransactionService.Services;

namespace TransactionService.Tests;

public class InMemoryTransactionStoreTests
{
    private readonly InMemoryTransactionStore _store = new();

    [Fact]
    public async Task CreateAndGetById_ShouldWorkCorrectly()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            TransactionDate = DateTime.UtcNow.AddMinutes(-1),
            Amount = 100
        };

        // Act
        var createResult = await _store.CreateAsync(transaction, CancellationToken.None);
        var retrieved = await _store.GetByIdAsync(transaction.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal(transaction.Id, retrieved!.Id);
        Assert.Equal(transaction.Amount, retrieved.Amount);
        Assert.Equal(transaction.TransactionDate, retrieved.TransactionDate);
        Assert.True(createResult.InsertDateTime <= DateTime.UtcNow);
    }

    [Fact]
    public async Task Create_ShouldBeIdempotent()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            TransactionDate = DateTime.UtcNow,
            Amount = 99
        };

        // Act
        var first = await _store.CreateAsync(transaction, CancellationToken.None);
        await Task.Delay(10);
        var second = await _store.CreateAsync(transaction, CancellationToken.None);

        // Assert
        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.Equal(first.InsertDateTime, second.InsertDateTime);
    }

    [Fact]
    public async Task Store_ShouldNotExceed100Transactions()
    {
        // Arrange
        var ids = new List<Guid>();

        for (int i = 0; i < 101; i++)
        {
            var id = Guid.NewGuid();
            ids.Add(id);
            await _store.CreateAsync(new Transaction
            {
                Id = id,
                TransactionDate = DateTime.UtcNow,
                Amount = 10
            }, CancellationToken.None);
        }

        var oldestId = ids[0];

        // Act
        var oldest = await _store.GetByIdAsync(oldestId, CancellationToken.None);
        var remainingCount = 0;

        foreach (var id in ids.Skip(1))
        {
            var t = await _store.GetByIdAsync(id, CancellationToken.None);
            if (t is not null) remainingCount++;
        }

        // Assert
        Assert.Null(oldest);
        Assert.Equal(100, remainingCount);
    }
}
