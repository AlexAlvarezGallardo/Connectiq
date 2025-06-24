using FluentValidation.Results;
using System.Net;

namespace Connectiq.ProjectDefaults.Response.Mutation;

public class MutationResponse : IMutationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<ValidationFailure> Errors { get; set; } = [];
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    public static MutationResponse Ok(string? message = null)
        => new() { Success = true, Message = message ?? string.Empty };

    public static MutationResponse Error(List<ValidationFailure>? errors = null, string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new() { Success = false, Message = message ?? string.Empty, Errors = errors ?? [], StatusCode = statusCode };
}


public class MutationResponse<T> : MutationResponse, IMutationResponse<T>
{
    public T? Data { get; set; }

    public static MutationResponse<T> Ok(T data, string? message = null)
        => new() { Success = true, Data = data, Message = message ?? string.Empty };

    public static MutationResponse<T> Error(T data, List<ValidationFailure>? errors = null, string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new() { Success = false, Data = data, Message = message ?? string.Empty, Errors = errors ?? [], StatusCode = statusCode };
}