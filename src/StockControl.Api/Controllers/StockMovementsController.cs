using Microsoft.AspNetCore.Mvc;
using StockControl.Application.Contracts;
using StockControl.Application.DTOs.StockMovements;
using StockControl.Application.Services.StockMovementServices;

namespace StockControl.Api.Controllers;

[Route("api/stock-movements")]
public class StockMovementsController : BaseApiController
{
    private readonly CreateStockMovementService _createStockMovementService;
    private readonly GetStockMovementByIdService _getStockMovementByIdService;
    private readonly ListStockMovementsService _listStockMovementsService;

    public StockMovementsController(
        CreateStockMovementService createStockMovementService,
        GetStockMovementByIdService getStockMovementByIdService,
        ListStockMovementsService listStockMovementsService)
    {
        _createStockMovementService = createStockMovementService;
        _getStockMovementByIdService = getStockMovementByIdService;
        _listStockMovementsService = listStockMovementsService;
    }

    [HttpPost("manual")]
    [ProducesResponseType(typeof(ApiResponse<StockMovementResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<StockMovementResponseDto>>> CreateManual([FromBody] CreateStockMovementDto dto, CancellationToken cancellationToken)
    {
        dto.TraceId = TraceId;
        var result = await _createStockMovementService.ExecuteAsync(dto, cancellationToken);
        return OkResponse(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<StockMovementResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<StockMovementResponseDto>>>> List(CancellationToken cancellationToken)
    {
        var result = await _listStockMovementsService.ExecuteAsync(cancellationToken);
        return OkResponse(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<StockMovementResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<StockMovementResponseDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getStockMovementByIdService.ExecuteAsync(id, cancellationToken);
        return OkResponse(result);
    }
}
