namespace Connectiq.ProjectDefaults.Response.Query;

public interface IQueryResponse
{
    bool Success { get; set; }
    string? Message { get; set; }
}

public interface IQueryResponse<T> : IQueryResponse
{
    T? Data { get; set; }
}

