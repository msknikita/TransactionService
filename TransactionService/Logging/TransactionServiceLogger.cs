namespace TransactionService.Logging;

public static class TransactionServiceLogger
{
    public static ILoggerFactory CreateFactory() =>
        LoggerFactory.Create(builder =>
        {
            builder
                .ClearProviders()
                .AddConsole()
                .SetMinimumLevel(LogLevel.Information);
        });
}