namespace Connectiq.ProjectDefaultsUnitTest;

using Connectiq.ProjectDefaults.LinqExtensions;
using Xunit;

public class CustomerEntity
{
    public required string Name { get; set; }
    public required string Email { get; set; }
}

public class CustomerFilter
{
    public required string Name { get; set; }
    public required string Email { get; set; }
}

public class LinqExtensionsTests
{
    [Fact]
    public void Build_NullFilter_ReturnsTruePredicate()
    {
        var linq = new LinqExtensions<CustomerEntity>();
        var predicate = linq.Build<CustomerFilter>(null!);

        var customer = new CustomerEntity { Name = "A", Email = "B" };
        Assert.True(predicate.Compile()(customer));
    }

    [Fact]
    public void Build_FilterByName_ReturnsPredicateThatMatchesName()
    {
        var linq = new LinqExtensions<CustomerEntity>();
        var filter = new CustomerFilter { Name = "John", Email = "john@gmail.com" };
        var predicate = linq.Build(filter);

        var match = new CustomerEntity { Name = "John", Email = "john@gmail.com" };
        var noMatch = new CustomerEntity { Name = "Jane", Email = "x" };

        Assert.True(predicate.Compile()(match));
        Assert.False(predicate.Compile()(noMatch));
    }

    [Fact]
    public void Build_FilterByNameAndEmail_ReturnsPredicateThatMatchesBoth()
    {
        var linq = new LinqExtensions<CustomerEntity>();
        var filter = new CustomerFilter { Name = "John", Email = "john@" };
        var predicate = linq.Build(filter);

        var match = new CustomerEntity { Name = "John", Email = "john@example.com" };
        var noMatch = new CustomerEntity { Name = "John", Email = "jane@example.com" };

        Assert.True(predicate.Compile()(match));
        Assert.False(predicate.Compile()(noMatch));
    }
}