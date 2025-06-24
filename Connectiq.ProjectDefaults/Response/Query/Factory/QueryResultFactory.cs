using System.Net;

namespace Connectiq.ProjectDefaults.Response.Query.Factory;

public class QueryResultFactory : IQueryResultFactory
{
    public IQueryResponse<T> Ok<T>(T data)
        => QueryResponse<T>.Ok(data);

    public IQueryResponse Error(string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => QueryResponse.Error(message, statusCode);

    public IQueryResponse<T> Error<T>(string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => QueryResponse<T>.Error(message, statusCode);
}
