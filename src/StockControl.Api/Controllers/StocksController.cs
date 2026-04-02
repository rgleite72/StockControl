using Microsoft.AspNetCore.Mvc;
using StockControl.Application.Contracts;
using StockControl.Application.DTOs.Stocks;
using StockControl.Application.Services.StockItemServices;

namespace StockControl.Api.Controllers;

[Route("api/stocks")]
public class StocksController : BaseApiController
{
    private readonly CreateStockItemService _createStockService;
    private readonly GetStockItemByIdService _getStockByIdService;
    private readonly GetStockItemByProductIdService _getStockByProductIdService;
    private readonly ListStockItemsService _listStocksService;

    public StocksController(
        CreateStockItemService createStockService,
        GetStockItemByIdService getStockByIdService,
        GetStockItemByProductIdService getStockByProductIdService,
        ListStockItemsService listStocksService)
    {
        _createStockService = createStockService;
        _getStockByIdService = getStockByIdService;
        _getStockByProductIdService = getStockByProductIdService;
        _listStocksService = listStocksService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<StockResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<StockResponseDto>>> Create([FromBody] CreateStockDto dto, CancellationToken cancellationToken)
    {
        var result = await _createStockService.ExecuteAsync(dto, cancellationToken);
        return OkResponse(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<StockResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<StockResponseDto>>>> List(CancellationToken cancellationToken)
    {
        var result = await _listStocksService.ExecuteAsync(cancellationToken);
        return OkResponse(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<StockResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<StockResponseDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getStockByIdService.ExecuteAsync(id, cancellationToken);
        return OkResponse(result);
    }

    [HttpGet("product/{productId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<StockResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<StockResponseDto>>> GetByProductId(Guid productId, CancellationToken cancellationToken)
    {
        var result = await _getStockByProductIdService.ExecuteAsync(productId, cancellationToken);
        return OkResponse(result);
    }
}
