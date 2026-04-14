namespace StockControl.Contracts.Events;

public sealed class StockMovementCreatedEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public string EventType { get; init; } = "stock.movement.created";
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    public Guid MovementId { get; init; }
    public Guid ProductId { get; init; }
    public string MovementType { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public string Source { get; init; } = "stock-api";
    public string CorrelationId { get; init; } = string.Empty;
}
