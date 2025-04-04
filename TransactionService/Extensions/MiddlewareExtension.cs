using TransactionService.Middlewares;

namespace TransactionService.Extensions;

public static class MiddlewareExtension
{
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<CustomMiddleware>()
            .UseHttpsRedirection()
            .UseRouting()
            .UseCustomSwagger(env);

        return app;
    }

    private static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app, IHostEnvironment env)
        => !env.IsProduction()
            ? app.UseSwagger()
                .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "TransactionService"); })
            : app;
}