using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Infrastructure.Messaging;
using StockControl.Application.Abstractions.Messaging;
using StockControl.Application.Interfaces;
using StockControl.Application.Interfaces.Repositories;
using StockControl.Infrastructure.Messaging;
using StockControl.Infrastructure.Repositories;

namespace StockControl.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<ServiceBusOptions>(
            configuration.GetSection(ServiceBusOptions.SectionName));

        var serviceBusOptions = configuration
            .GetSection(ServiceBusOptions.SectionName)
            .Get<ServiceBusOptions>() ?? new ServiceBusOptions();

        services.AddSingleton(_ => new ServiceBusClient(serviceBusOptions.ConnectionString));

        services.AddScoped<IStockEventPublisher, StockEventPublisher>();
        services.AddScoped<IStockItemRepository, StockItemRepository>();
        services.AddScoped<IStockMovementRepository, StockMovementRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
