using Microsoft.AspNetCore.Http;
using StockControl.Application.Abstractions.Messaging;
using StockControl.Application.DTOs.StockMovements;
using StockControl.Application.Exceptions;
using StockControl.Application.Interfaces;
using StockControl.Application.Interfaces.Repositories;
using StockControl.Contracts.Events;
using StockControl.Domain.Entities;
using StockControl.Domain.Enums;

namespace StockControl.Application.Services.StockMovementServices;

public class CreateStockMovementService
{
    private readonly IStockItemRepository _stockRepository;
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStockEventPublisher _stockEventPublisher;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateStockMovementService(
        IStockItemRepository stockRepository,
        IStockMovementRepository stockMovementRepository,
        IUnitOfWork unitOfWork,
        IStockEventPublisher stockEventPublisher,
        IHttpContextAccessor httpContextAccessor)
    {
        _stockRepository = stockRepository;
        _stockMovementRepository = stockMovementRepository;
        _unitOfWork = unitOfWork;
        _stockEventPublisher = stockEventPublisher;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<StockMovementResponseDto> ExecuteAsync(CreateStockMovementDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.ProductId == Guid.Empty)
        {
            throw ValidationException.ForField(nameof(dto.ProductId), "ProductId is required.");
        }

        if (dto.Quantity <= 0)
        {
            throw ValidationException.ForField(nameof(dto.Quantity), "Quantity must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(dto.Reason))
        {
            throw ValidationException.ForField(nameof(dto.Reason), "Reason is required.");
        }

        var stock = await _stockRepository.FindByProductIdAsync(dto.ProductId, cancellationToken);
        if (stock is null)
        {
            throw new NotFoundException("StockItem not found for this product.");
        }

        var movementType = ParseMovementType(dto.MovementType);
        var previousQuantity = stock.QuantityAvailable;
        var currentQuantity = previousQuantity;

        switch (movementType)
        {
            case StockMovementType.In:
                currentQuantity = previousQuantity + dto.Quantity;
                break;

            case StockMovementType.Out:
                currentQuantity = previousQuantity - dto.Quantity;
                if (currentQuantity < 0)
                {
                    throw new ConflictException("Insufficient stock for this movement.");
                }
                break;

            case StockMovementType.Adjust:
                currentQuantity = dto.Quantity;
                break;
        }

        stock.QuantityAvailable = currentQuantity;
        stock.UpdatedAt = DateTime.UtcNow;

        var correlationId = _httpContextAccessor.HttpContext?.Items["X-Request-Id"]?.ToString()
            ?? _httpContextAccessor.HttpContext?.TraceIdentifier
            ?? Guid.NewGuid().ToString("N");

        var stockMovement = new StockMovement
        {
            Id = Guid.NewGuid(),
            StockId = stock.Id,
            ProductId = stock.ProductId,
            MovementType = movementType,
            Quantity = dto.Quantity,
            PreviousQuantity = previousQuantity,
            CurrentQuantity = currentQuantity,
            Reason = dto.Reason,
            TraceId = correlationId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _stockRepository.Update(stock);
            await _stockMovementRepository.CreateAsync(stockMovement, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }

        var @event = new StockMovementCreatedEvent
        {
            MovementId = stockMovement.Id,
            ProductId = stockMovement.ProductId,
            MovementType = stockMovement.MovementType.ToString(),
            Quantity = stockMovement.Quantity,
            CorrelationId = correlationId
        };

        await _stockEventPublisher.PublishStockMovementCreatedAsync(@event, cancellationToken);

        return new StockMovementResponseDto
        {
            Id = stockMovement.Id,
            StockId = stockMovement.StockId,
            ProductId = stockMovement.ProductId,
            MovementType = stockMovement.MovementType.ToString(),
            Quantity = stockMovement.Quantity,
            PreviousQuantity = stockMovement.PreviousQuantity,
            CurrentQuantity = stockMovement.CurrentQuantity,
            Reason = stockMovement.Reason,
            TraceId = stockMovement.TraceId,
            CreatedAt = stockMovement.CreatedAt
        };
    }

    private static StockMovementType ParseMovementType(string movementType)
    {
        if (string.IsNullOrWhiteSpace(movementType))
        {
            throw ValidationException.ForField(nameof(movementType), "MovementType is required.");
        }

        if (Enum.TryParse<StockMovementType>(movementType, true, out var parsedMovementType))
        {
            return parsedMovementType;
        }

        throw ValidationException.ForField(nameof(movementType), "MovementType must be In, Out or Adjust.");
    }
}
