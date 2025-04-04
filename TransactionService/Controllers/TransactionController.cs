namespace TransactionService.Controllers;

[ApiController]
[Route("api/v1/Transaction")]
public class TransactionController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateTransaction.Response), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<CreateTransaction.Response> CreateTransaction([FromBody] CreateTransaction.Command command,
        CancellationToken cts) =>
        await mediator.Send(command, cts);

    [HttpGet]
    [ProducesResponseType(typeof(Transaction), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<ActionResult<Transaction>> GetTransaction([FromQuery] Guid id, CancellationToken cts)
    {
        var result = await mediator.Send(new ReadTransactions.Query { Id = id }, cts);

        if (result is null)
            return NotFound(new ProblemDetails
            {
                Status = 404,
                Title = "Not Found",
                Detail = $"Transaction with ID {id} not found"
            });

        return Ok(result);
    }   
}