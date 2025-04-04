namespace TransactionService.Model.Transactions.Queries;

public static class ReadTransactions
{
    public record Query : IRequest<Transaction>
    {
        public Guid Id { get; init; }
    }

    public class Handler(ITransactionStore store) : IRequestHandler<Query, Transaction>
    {
        public async Task<Transaction?> Handle(Query request, CancellationToken ct) => await store.GetByIdAsync(request.Id, ct);
    }
}
