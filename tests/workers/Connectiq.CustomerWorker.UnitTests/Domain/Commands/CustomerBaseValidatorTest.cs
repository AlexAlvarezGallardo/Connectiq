using CustomerWorker.Domain.Commands;
using FluentValidation.Validators;
using FluentValidation.Validators.UnitTestExtension.Composer;
using FluentValidation.Validators.UnitTestExtension.Core;
using Xunit;

namespace Connectiq.CustomerWorker.UnitTests.Domain.Commands;

public class CustomerBaseValidatorTest
{
    readonly CustomerBaseValidator _customerBaseValidator = new();

    [Fact]
    public void When_CustomerBaseValidatorConstructing_Then_3PropertiesShouldHaveRules()
    {
        _customerBaseValidator.ShouldHaveRulesCount(3);
    }

    [Fact]
    public void When_CustomerBaseValidatorConstructing_Then_RulesAreConfiguredCorrectly()
    {
        _customerBaseValidator.ShouldHaveRules(x => x.CustomerValidated,
            BaseVerifiersSetComposer.Build()
            .AddChildValidatorVerifier<CustomerValidatedValidator, CustomerBase, CustomerValidated>()
            .Create());

        _customerBaseValidator.ShouldHaveRules(x => x.EventId,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerBase, string>>()
             .Create());

        _customerBaseValidator.ShouldHaveRules(x => x.IsActive,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerBase, bool>>()
             .Create());
    }
}