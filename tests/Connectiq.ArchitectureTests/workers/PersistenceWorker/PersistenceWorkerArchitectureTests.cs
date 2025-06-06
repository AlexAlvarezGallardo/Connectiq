using FluentAssertions;
using NetArchTest.Rules;
using PersistenceWorker.Repository;
using System.Reflection;

namespace Connectiq.ArchitectureTests;

public class PersistenceWorkerArchitectureTests
{
    private readonly Assembly _assembly = Assembly.Load("PersistenceWorker");

    [Fact]
    public void PersistenceWorker_Should_Not_Depend_On_API_Or_Worker_Projects()
    {
        var result = Types
            .InAssembly(_assembly)
            .ShouldNot().HaveDependencyOn("Connectiq.API")
            .And().HaveDependencyOn("CustomerWorker")
            .GetResult();

        result.IsSuccessful
            .Should()
            .BeTrue("because PersistenceWorker should not depend on API or other worker projects.");
    }

    [Fact]
    public void Services_And_Repositories_Should_Have_Interfaces()
    {
        var result = Types
            .InAssembly(_assembly)
            .That().HaveNameEndingWith("Service")
            .Or().HaveNameEndingWith("Repository")
            .Should().ImplementInterface(typeof(IRepository<>))
            .GetResult();

        result.IsSuccessful
            .Should()
            .BeTrue("All services and repositories should implement an interface for DI.");
    }
}