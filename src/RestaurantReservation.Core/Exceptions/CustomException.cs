using System.Net;

namespace RestaurantReservation.Core.Exceptions;

public class CustomException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public int? Code { get; }

    protected CustomException(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        int? code = null) : base(message)
    {
        this.StatusCode = statusCode;
        this.Code = code;
    }

    protected CustomException(
        string message,
        System.Exception innerException,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        int? code = null) : base(message, innerException)
    {
        this.StatusCode = statusCode;
        this.Code = code;
    }

    protected CustomException(
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        int? code = null)
    {
        this.StatusCode = statusCode;
        this.Code = code;
    }
}