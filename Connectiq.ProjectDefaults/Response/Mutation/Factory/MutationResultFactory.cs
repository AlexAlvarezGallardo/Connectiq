using FluentValidation.Results;
using System.Net;

namespace Connectiq.ProjectDefaults.Response.Mutation.Factory;

public class MutationResultFactory : IMutationResultFactory
{
    public MutationResponse Ok(string? message = null) =>
        MutationResponse.Ok(message);

    public MutationResponse<T> Ok<T>(T data, string? message = null) =>
        MutationResponse<T>.Ok(data, message);

    public MutationResponse Error(List<ValidationFailure>? errors = null, string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => MutationResponse.Error(errors, message, statusCode);

    public MutationResponse<T> Error<T>(T data, List<ValidationFailure>? errors = null, string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => MutationResponse<T>.Error(data, errors, message, statusCode);
}
