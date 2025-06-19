namespace DatabaseWorker.Domain.DbSeeder;

public class DbSeederOptions
{
    public SeedEntityOptions Default { get; set; } = new();
    public Dictionary<string, SeedEntityOptions> Entities { get; set; } = [];
}
