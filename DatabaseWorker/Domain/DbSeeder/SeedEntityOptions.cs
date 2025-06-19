namespace DatabaseWorker.Domain.DbSeeder;

public class SeedEntityOptions
{
    public SeedDataSourceType SeedDataSource { get; set; } = SeedDataSourceType.Bogus; 
    public int? Number { get; set; }
    public string? EntityFilePath { get; set; }
}

public enum SeedDataSourceType
{
    Bogus,
    Json
}
