using Financial.Transaction.API.Middlewares;
using Financial.Transaction.API.Services;
using Financial.Transaction.API.Settings;

namespace Financial.Transaction.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddServices();
        builder.Services.AddHealthChecks();
        builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        builder.Services.Configure<StatisticsSettings>(
            builder.Configuration.GetSection(StatisticsSettings.Position)
        );

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
            app.MapOpenApi();

        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();
        app.MapHealthChecks("health");

        app.Run();
    }
}
