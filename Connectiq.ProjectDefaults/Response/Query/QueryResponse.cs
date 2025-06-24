using System.Net;

namespace Connectiq.ProjectDefaults.Response.Query;

public class QueryResponse : IQueryResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; } = string.Empty;
    public HttpStatusCode StatusCode { get; set; }

    public static QueryResponse Ok() => new()
    {
        Success = true,
        StatusCode = HttpStatusCode.OK
    };

    public static QueryResponse Error(string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new()
    {
        Success = false,
        Message = message,
        StatusCode = statusCode
    };
}


public class QueryResponse<T> : IQueryResponse<T>
{
    public T? Data { get; set; }

    public bool Success { get; set; }

    public string? Message { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public static QueryResponse<T> Ok(T data) => new()
    {
        Success = true,
        StatusCode = HttpStatusCode.OK,
        Data = data
    };

    public static QueryResponse<T> Error(string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new()
    {
        Success = false,
        Message = message,
        StatusCode = statusCode,
        Data = default
    };
}
