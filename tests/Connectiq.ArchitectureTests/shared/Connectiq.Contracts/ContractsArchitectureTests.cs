using CustomerWorker.Domain;
using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;

namespace Connectiq.ArchitectureTests.shared.Connectiq.Contracts;

public class ContractsArchitectureTests 
{
    private readonly Assembly _assembly = Assembly.Load("Connectiq.Contracts");

    [Fact]
    public void Contracts_Should_Not_Have_Dependencies_On_Other_Projects()
    {
        var result = Types
            .InAssembly(_assembly)
            .ShouldNot()
            .HaveDependencyOn("CustomerWorker")
            .And()
            .HaveDependencyOn("PersistenceWorker")
            .And()
            .HaveDependencyOn("Connectiq.API")
            .GetResult();

        result.IsSuccessful
            .Should()
            .BeTrue("because the Contracts project should not depend on other projects in the solution.");
    }

    [Theory]
    [InlineData(typeof(CustomerEntity), "Customer")]
    public void Every_Folder_Should_Contain_Entity_Validator_MapperProfile_And_Proto(Type entityType, string folderName)
    {
        ArgumentNullException.ThrowIfNull(entityType);
        ArgumentNullException.ThrowIfNull(folderName);

        var allTypes = Types
            .InAssembly(_assembly)
            .That()
            .ResideInNamespaceMatching("Connectiq.Contracts.*")
            .GetTypes();

        var byFolder = allTypes
            .GroupBy(t => t.Namespace?.Split('.').Last())
            .ToList();

        foreach (var folder in byFolder)
        {
            var types = folder.ToList();

            types.Should()
                .Contain(t => t.Name.EndsWith("Entity"), $"The folder {folderName} must contain at least one class ending with 'Entity'.");
            types.Should()
                .Contain(t => t.Name.EndsWith("Validator"), $"The folder {folderName} must contain at least one class ending with 'Validator'.");
            types.Should()
                .Contain(t => t.Name.EndsWith("MapperProfile"), $"The folder {folderName} must contain at least one class ending with 'MapperProfile'.");

            var baseDir = Path.Combine(AppContext.BaseDirectory, folderName!);
            var protoFiles = Directory.Exists(baseDir) ? Directory.GetFiles(baseDir, "*.proto") : [];
            
            protoFiles
                .Should()
                .NotBeEmpty($"The folder {folderName} must contain at least one .proto file.");
        }
    }
}