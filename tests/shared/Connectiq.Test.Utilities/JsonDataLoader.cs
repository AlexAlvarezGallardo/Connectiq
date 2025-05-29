using System.Text.Json;

namespace Connectiq.Tests.Utilities;

public static class JsonDataLoader
{
    public static TOutput LoadFromFile<TOutput>(string relativePath)
    {
        var basePath = Directory.GetCurrentDirectory();
        var fullPath = Path.Combine(basePath, relativePath);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Test data file not found: {fullPath}");

        var json = File.ReadAllText(fullPath);
        return JsonSerializer.Deserialize<TOutput>(json)
               ?? throw new InvalidOperationException("Deserialization returned null");
    }

    public static string GetDataPath(string jsonName)
    {
        var basePath = Directory.GetCurrentDirectory();
        return Path.Combine(basePath, "TestData", "Customers", jsonName);
    }
}