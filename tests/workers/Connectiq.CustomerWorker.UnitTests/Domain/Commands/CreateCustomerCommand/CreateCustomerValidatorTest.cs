using CustomerWorker.Domain.Commands;
using CustomerWorker.Domain.Commands.CreateCustomerCommand;
using FluentValidation.Validators.UnitTestExtension.Composer;
using FluentValidation.Validators.UnitTestExtension.Core;
using Xunit;

namespace Connectiq.CustomerWorker.UnitTests.Domain.Commands.CreateCustomerCommand;

public class CreateCustomerValidatorTest
{
    readonly CreateCustomerValidator _createCustomerValidator = new();

    [Fact]
    public void When_CreateCustomerValidatorConstructing_Then_1PropertiesShouldHaveRules()
    {
        _createCustomerValidator.ShouldHaveRulesCount(1);
    }

    [Fact]
    public void When_CreateCustomerValidatorConstructing_Then_RulesAreConfiguredCorrectly()
    {
        _createCustomerValidator.ShouldHaveRules(x => x,
            BaseVerifiersSetComposer.Build()
            .AddChildValidatorVerifier<CustomerBaseValidator, CreateCustomer, CreateCustomer>()
            .Create());
    }
}