using DatabaseWorker.Domain.DbSeeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace DatabaseWorker.Services;

public class DbSeeder<TContext, TEntity>(
    ILogger<DbSeeder<TContext, TEntity>> _logger,
    IOptions<DbSeederOptions> _options,
    JsonSerializerOptions _jsonSerializerOptions
) : IDbSeeder<TContext>
    where TContext : DbContext
    where TEntity : class
{
    public async Task SeedAsync(TContext context)
    {
        var dbSet = context.Set<TEntity>();

        if (await dbSet.AnyAsync())
        {
            _logger.LogInformation("Ya existen datos en la tabla {Entity}. Seeder omitido.", typeof(TEntity).Name);
            return;
        }

        var entityName = typeof(TEntity).Name;

        if (!_options.Value.EntityFilePaths.TryGetValue(entityName, out var jsonPath))
        {
            _logger.LogWarning("No se encontró ruta de seed configurada para {Entity}", entityName);
            return;
        }

        if (!File.Exists(jsonPath))
        {
            _logger.LogWarning("Archivo JSON no encontrado en {Path}", jsonPath);
            return;
        }

        var json = await File.ReadAllTextAsync(jsonPath);
        var entities = JsonSerializer.Deserialize<List<TEntity>>(json, _jsonSerializerOptions);

        if (entities is not null && entities.Any())
        {
            await dbSet.AddRangeAsync(entities);
            await context.SaveChangesAsync();
            _logger.LogInformation("Insertados {Count} registros en {Entity}", entities.Count, entityName);
        }
    }
}
