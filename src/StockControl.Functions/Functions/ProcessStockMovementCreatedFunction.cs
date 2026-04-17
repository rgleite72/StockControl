using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using StockControl.Contracts.Events;

namespace StockControl.Functions.Functions;

public class ProcessStockMovementCreatedFunction
{
    private readonly ILogger<ProcessStockMovementCreatedFunction> _logger;

    public ProcessStockMovementCreatedFunction(
        ILogger<ProcessStockMovementCreatedFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(ProcessStockMovementCreatedFunction))]
    public async Task Run(
        [ServiceBusTrigger("%ServiceBusQueueName%", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        CancellationToken cancellationToken)
    {
        try
        {
            var body = message.Body.ToString();

            var @event = JsonSerializer.Deserialize<StockMovementCreatedEvent>(
                body,
                new JsonSerializerOptions(JsonSerializerDefaults.Web));

            if (@event is null)
            {
                _logger.LogError(
                    "Message received but could not deserialize body. MessageId: {MessageId}",
                    message.MessageId);

                await messageActions.DeadLetterMessageAsync(
                    message,
                    deadLetterReason: "InvalidPayload",
                    deadLetterErrorDescription: "Body could not be deserialized to StockMovementCreatedEvent",
                    cancellationToken: cancellationToken);

                return;
            }

            _logger.LogInformation(
                "Stock event received. EventId: {EventId}, MovementId: {MovementId}, ProductId: {ProductId}, CorrelationId: {CorrelationId}, EventType: {EventType}",
                @event.EventId,
                @event.MovementId,
                @event.ProductId,
                @event.CorrelationId,
                @event.EventType);

            // Aqui hoje fica o processamento inicial.
            // Ex.: validar, enriquecer, integrar, salvar auditoria etc.

            await Task.CompletedTask;

            await messageActions.CompleteMessageAsync(message, cancellationToken);

            _logger.LogInformation(
                "Stock event processed successfully. MessageId: {MessageId}",
                message.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error processing stock event. MessageId: {MessageId}",
                message.MessageId);

            throw;
        }
    }
}
