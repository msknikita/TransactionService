namespace TransactionService.Model.Transactions.Commands;

public static class CreateTransaction
{
    public record Command : IRequest<Response>
    {
        public Guid Id { get; init; }
        public DateTime TransactionDate { get; init; }
        public decimal Amount { get; init; }
    }

    public record Response(DateTime InsertDateTime);

    public class Handler(ITransactionStore store) : IRequestHandler<Command, Response>
    {
        public async Task<Response> Handle(Command request, CancellationToken ct)
        {
            if (request.Amount <= 0)
                throw new ArgumentException("Amount must be positive");

            if (request.TransactionDate > DateTime.UtcNow)
                throw new ArgumentException("Transaction date cannot be in the future");

            var transaction = new Transaction
            {
                Id = request.Id,
                Amount = request.Amount,
                TransactionDate = request.TransactionDate
            };

            var result = await store.CreateAsync(transaction, ct);

            return new Response(result.InsertDateTime);
        }
    }
}
