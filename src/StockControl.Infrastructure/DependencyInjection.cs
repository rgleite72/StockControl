using Microsoft.Extensions.DependencyInjection;
using StockControl.Application.Interfaces;
using StockControl.Application.Interfaces.Repositories;
using StockControl.Infrastructure.Persistence;
using StockControl.Infrastructure.Repositories;

namespace StockControl.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IStockItemRepository, StockItemRepository>();
        services.AddScoped<IStockMovementRepository, StockMovementRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
