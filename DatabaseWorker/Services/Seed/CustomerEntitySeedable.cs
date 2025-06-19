namespace DatabaseWorker.Services.Seed;

public class CustomerEntitySeedable : ISeedableEntity<CustomerEntity>
{
    public static IEnumerable<CustomerEntity> GenerateSeedData(int count)
    {
        var faker = new Bogus.Faker<CustomerEntity>()
            .RuleFor(c => c.Id, _ => Guid.NewGuid())
            .RuleFor(c => c.Name, f => f.Name.FullName())
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .RuleFor(c => c.Phone, f => Truncate(f.Phone.PhoneNumber(), 20))
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.CreatedAt, f => f.Date.PastOffset(2).ToUniversalTime())
            .RuleFor(c => c.IsActive, f => f.Random.Bool(0.8f)); 

        return faker.Generate(count);
    }

    private static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        return value.Length <= maxLength ? value : value[..maxLength];
    }
}