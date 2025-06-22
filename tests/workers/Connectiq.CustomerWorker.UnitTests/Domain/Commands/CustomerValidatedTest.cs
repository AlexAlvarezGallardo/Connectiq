using CustomerWorker.Domain.Commands;
using FluentValidation.Validators;
using FluentValidation.Validators.UnitTestExtension.Composer;
using FluentValidation.Validators.UnitTestExtension.Core;
using Xunit;

namespace Connectiq.CustomerWorker.UnitTests.Domain.Commands;

public class CustomerValidatedTest
{
    readonly CustomerValidatedValidator customerValidatedValidator = new();

    [Fact]
    public void When_CustomerValidatedValidatorConstructing_Then_3PropertiesShouldHaveRules()
    {
        customerValidatedValidator.ShouldHaveRulesCount(3);
    }

    [Fact]
    public void When_CustomerValidatedValidatorConstructing_Then_RulesAreConfiguredCorrectly()
    {
        customerValidatedValidator.ShouldHaveRules(x => x.Customer,
            BaseVerifiersSetComposer.Build()
                .AddChildValidatorVerifier<CustomerValidator, CustomerValidated, Customers.Customer>()
                .Create());

        customerValidatedValidator.ShouldHaveRules(x => x.CreatedAt,
            BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerValidated, DateTimeOffset>>()
                .AddPropertyValidatorVerifier<PredicateValidator<CustomerValidated, DateTimeOffset>>()
                .Create());

        customerValidatedValidator.ShouldHaveRules(x => x.IsValid,
            BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerValidated, bool>>()
                .Create());
    }
}
