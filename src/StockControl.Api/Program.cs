using Microsoft.EntityFrameworkCore;
using StockControl.Api.Extensions;
using StockControl.Api.Middlewares;
using StockControl.Application.Services.StockMovementServices;
using StockControl.Application.Services.StockItemServices;
using StockControl.Infrastructure;
using StockControl.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = true;
});

builder.Services.AddControllers();
builder.Services.AddCustomApiBehavior();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "StockControl API",
        Version = "v1",
        Description = "API for stock management"
    });
});

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não foi configurada.");

builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString);

builder.Services.AddInfrastructure();

builder.Services.AddScoped<CreateStockItemService>();
builder.Services.AddScoped<GetStockItemByIdService>();
builder.Services.AddScoped<GetStockItemByProductIdService>();
builder.Services.AddScoped<ListStockItemsService>();
builder.Services.AddScoped<CreateStockMovementService>();
builder.Services.AddScoped<GetStockMovementByIdService>();
builder.Services.AddScoped<ListStockMovementsService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "StockControl API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseMiddleware<RequestCorrelationMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Use(async (ctx, next) =>
{
    var logger = ctx.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("Request");
    var traceId = RequestCorrelationMiddleware.GetRequestId(ctx);

    logger.LogInformation("HTTP {Method} {Path} TraceId={TraceId}",
        ctx.Request.Method, ctx.Request.Path, traceId);

    try
    {
        await next();
    }
    finally
    {
        logger.LogInformation("HTTP {StatusCode} {Method} {Path} TraceId={TraceId}",
            ctx.Response.StatusCode, ctx.Request.Method, ctx.Request.Path, traceId);
    }
});

app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");
app.Run();
