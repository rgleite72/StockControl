using Microsoft.AspNetCore.Mvc;
using StockControl.Application.Contracts;
using StockControl.Api.Middlewares;

namespace StockControl.Api.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected string TraceId => RequestCorrelationMiddleware.GetRequestId(HttpContext);

    protected ActionResult<ApiResponse<T>> OkResponse<T>(T data)
        => Ok(ApiResponse<T>.Ok(data, TraceId));
}




