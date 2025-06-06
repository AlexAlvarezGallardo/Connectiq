using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;

namespace Connectiq.ArchitectureTests.workers.PersistenceWorker.Consumers;

public class ConsumerArchitectureTests
{
    private readonly Assembly _assembly = Assembly.Load("PersistenceWorker");

    [Fact]
    public void All_Classes_In_Consumers_Folder_Should_End_With_Event()
    {
        var failingTypes = Types
            .InAssembly(_assembly)
            .That()
            .AreClasses()
            .And()
            .ResideInNamespaceMatching(".*Consumer.*")
            .Should()
            .HaveNameEndingWith("Event")
            .GetTypes()
            .Where(t => !t.Name.EndsWith("Event"))
            .ToList();

        failingTypes
            .Should()
            .BeEmpty("all classes in ConsumerWorker should end with 'Event'");
    }
}