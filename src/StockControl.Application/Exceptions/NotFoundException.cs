using System.Net;

namespace StockControl.Application.Exceptions;

public sealed class NotFoundException : AppException
{
    public NotFoundException(string message, string code = "NOT_FOUND")
        : base(code, message, HttpStatusCode.NotFound) { }
}
