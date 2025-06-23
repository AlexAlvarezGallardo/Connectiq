using FluentValidation.Results;

namespace Connectiq.ProjectDefaults.Response.Mutation;

public interface IMutationResponse
{
    bool Success { get; set; }
    string Message { get; set; }
}

public interface IMutationResponse<T> : IMutationResponse
{
    T? Data { get; set; }

    List<ValidationFailure> Errors { get; set; }
}

