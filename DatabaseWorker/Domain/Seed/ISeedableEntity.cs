namespace DatabaseWorker.Domain.Seed;

public interface ISeedableEntity<TEntity>
    where TEntity : class
{
    static abstract IEnumerable<TEntity> GenerateSeedData(int count);

    static virtual async Task<IEnumerable<TEntity>> LoadFromJsonAsync(string path, JsonSerializerOptions options)
    {
        if (!File.Exists(path))
            return [];

        var json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<List<TEntity>>(json, options) ?? [];
    }
}

