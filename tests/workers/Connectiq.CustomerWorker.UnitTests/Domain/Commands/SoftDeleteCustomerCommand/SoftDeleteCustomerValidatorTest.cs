using CustomerWorker.Domain.Commands.SoftDeleteCustomerCommand;
using FluentValidation.Validators;
using FluentValidation.Validators.UnitTestExtension.Composer;
using FluentValidation.Validators.UnitTestExtension.Core;
using Xunit;

namespace Connectiq.CustomerWorker.UnitTests.Domain.Commands.SoftDeleteCustomerCommand;

public class SoftDeleteCustomerValidatorTest
{
    readonly SoftDeleteCustomerValidator _softDeleteCustomerValidator = new();

    [Fact]
    public void When_SoftDeleteCustomerValidatorConstructing_Then_1PropertiesShouldHaveRules()
    {
        _softDeleteCustomerValidator.ShouldHaveRulesCount(1);
    }

    [Fact]
    public void When_SoftDeleteCustomerValidatorConstructing_Then_RulesAreConfiguredCorrectly()
    {
        _softDeleteCustomerValidator.ShouldHaveRules(x => x.Id,
            BaseVerifiersSetComposer.Build()
            .AddPropertyValidatorVerifier<NotEmptyValidator<SoftDeleteCustomer, Guid>>()
            .Create());
    }

}
