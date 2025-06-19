namespace DatabaseWorker.Services.DbSeeder;

public class DbSeeder<TContext, TEntity, TSeedable>(
    ILogger<DbSeeder<TContext, TEntity, TSeedable>> _logger,
    IOptions<DbSeederOptions> _options,
    JsonSerializerOptions _jsonOptions
) : IDbSeeder<TContext>
    where TContext : DbContext
    where TEntity : class
    where TSeedable : class, ISeedableEntity<TEntity>
{
    public async Task SeedAsync(TContext context)
    {
        var dbSet = context.Set<TEntity>();
        var entityName = typeof(TEntity).Name;

        if (await dbSet.AnyAsync())
        {
            _logger.LogInformation("Ya existen datos en la tabla {Entity}, se omite el seed.", entityName);
            return;
        }

        var config = _options.Value.Entities.GetValueOrDefault(entityName) ?? _options.Value.Default;
        var number = config.Number ?? _options.Value.Default.Number;

        IEnumerable<TEntity> entities = config.SeedDataSource switch
        {
            SeedDataSourceType.Json => await TSeedable.LoadFromJsonAsync(config.EntityFilePath!, _jsonOptions),
            SeedDataSourceType.Bogus => TSeedable.GenerateSeedData(number!.Value),
            _ => Enumerable.Empty<TEntity>()
        };

        if (!entities.Any())
        {
            _logger.LogWarning("No se generaron datos para {Entity}", entityName);
            return;
        }

        await dbSet.AddRangeAsync(entities);
        await context.SaveChangesAsync();

        _logger.LogInformation("Insertados {Count} registros en {Entity}", entities.Count(), entityName);
    }
}
