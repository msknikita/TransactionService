using var loggerFactory = TransactionServiceLogger.CreateFactory();
ILogger logger = loggerFactory.CreateLogger("TransactionService");

try
{
    logger.LogInformation("Web host is building ...");

    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddTransactionServices();

    var app = builder.Build();

    app.UseCustomMiddleware(app.Environment);
    app.MapControllers();
    
    logger.LogInformation("Web host is run ...");
    
    app.Run();
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Host terminated unexpectedly");
}
