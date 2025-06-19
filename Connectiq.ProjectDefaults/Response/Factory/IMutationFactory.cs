using Connectiq.ProjectDefaults.Response.Factory.Mutation;
using FluentValidation.Results;
using System.Net;

namespace Connectiq.ProjectDefaults.Response.Factory;

public interface IMutationResultFactory
{
    MutationResponse Ok(string? message = null);
    MutationResponse<T> Ok<T>(T data, string? message = null);
    MutationResponse Error(List<ValidationFailure>? errors = null, string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest);
    MutationResponse<T> Error<T>(T data, List<ValidationFailure>? errors = null, string? message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest);
}