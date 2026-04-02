using System.Net;

namespace StockControl.Application.Exceptions;

public sealed class ConflictException : AppException
{
    public ConflictException(string message, string code = "CONFLICT")
        : base(code, message, HttpStatusCode.Conflict) { }
}
