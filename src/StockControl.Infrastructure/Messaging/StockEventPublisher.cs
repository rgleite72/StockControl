using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockControl.Application.Abstractions.Messaging;
using StockControl.Contracts.Events;
using StockControl.Infrastructure.Messaging;

namespace Stock.Infrastructure.Messaging;

public sealed class StockEventPublisher : IStockEventPublisher
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly ServiceBusClient _serviceBusClient;
    private readonly ServiceBusOptions _options;
    private readonly ILogger<StockEventPublisher> _logger;

    public StockEventPublisher(
        ServiceBusClient serviceBusClient,
        IOptions<ServiceBusOptions> options,
        ILogger<StockEventPublisher> logger)
    {
        _serviceBusClient = serviceBusClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task PublishStockMovementCreatedAsync(
        StockMovementCreatedEvent @event,
        CancellationToken cancellationToken = default)
    {
        var sender = _serviceBusClient.CreateSender(_options.QueueName);

        var body = JsonSerializer.Serialize(@event, JsonOptions);

        var message = new ServiceBusMessage(body)
        {
            MessageId = @event.EventId.ToString(),
            Subject = @event.EventType,
            CorrelationId = @event.CorrelationId,
            ContentType = "application/json"
        };

        message.ApplicationProperties["eventType"] = @event.EventType;
        message.ApplicationProperties["movementId"] = @event.MovementId.ToString();
        message.ApplicationProperties["productId"] = @event.ProductId.ToString();

        _logger.LogInformation(
            "Publishing stock movement event. EventId: {EventId}, MovementId: {MovementId}, ProductId: {ProductId}, CorrelationId: {CorrelationId}",
            @event.EventId,
            @event.MovementId,
            @event.ProductId,
            @event.CorrelationId);

        await sender.SendMessageAsync(message, cancellationToken);

        _logger.LogInformation(
            "Stock movement event published successfully. EventId: {EventId}",
            @event.EventId);
    }
}
