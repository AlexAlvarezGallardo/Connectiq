using CustomerWorker.Domain.Commands;
using CustomerWorker.Domain.Commands.UpdateCustomerCommand;
using FluentValidation.Validators;
using FluentValidation.Validators.UnitTestExtension.Composer;
using FluentValidation.Validators.UnitTestExtension.Core;
using Xunit;

namespace Connectiq.CustomerWorker.UnitTests.Domain.Commands.UpdateCustomerCommand;

public class UpdateCustomerValidatorTest
{
    readonly UpdateCustomerValidator _updateCustomerValidator = new();

    [Fact]
    public void When_UpdateCustomerValidatorConstructing_Then_1PropertiesShouldHaveRules()
    {
        _updateCustomerValidator.ShouldHaveRulesCount(1);
    }

    [Fact]
    public void When_UpdateCustomerValidatorConstructing_Then_RulesAreConfiguredCorrectly()
    {
        _updateCustomerValidator.ShouldHaveRules(x => x,
            BaseVerifiersSetComposer.Build()
            .AddChildValidatorVerifier<CustomerBaseValidator, UpdateCustomer, UpdateCustomer>()
            .Create());
    }
}