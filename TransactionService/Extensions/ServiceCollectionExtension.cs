namespace TransactionService.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddTransactionServices(this IServiceCollection services) =>
        services
            .AddControllers()
            .Services
            .AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddSingleton<ITransactionStore, InMemoryTransactionStore>()
            .AddSwaggerGen()
            .AddEndpointsApiExplorer();
}