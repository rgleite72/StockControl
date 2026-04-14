using StockControl.Contracts.Events;

namespace StockControl.Application.Abstractions.Messaging;

public interface IStockEventPublisher
{
    Task PublishStockMovementCreatedAsync(
        StockMovementCreatedEvent @event,
        CancellationToken cancellationToken = default);
}
