using System.Net;

namespace Connectiq.ProjectDefaults.Response.Query.Factory;

public interface IQueryResultFactory
{
    IQueryResponse<T> Ok<T>(T data);
    IQueryResponse Error(string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest);
    IQueryResponse<T> Error<T>(string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest);
}
