using Connectiq.Contracts.Customer;
using FluentValidation.Validators;
using FluentValidation.Validators.UnitTestExtension.Composer;
using FluentValidation.Validators.UnitTestExtension.Core;
using Xunit;

namespace Connectiq.Contracts.Tests.Customer;

public class ValidatorTest
{
    readonly CustomerCreatedValidator customerCreatedValidator = new();
    readonly CustomerValidatedValidator customerValidatedValidator = new();

    [Fact]
    public void When_CustomerCreatedValidatorConstructing_Then_4PropertiesShouldHaveRules()
    {
        customerCreatedValidator.ShouldHaveRulesCount(4);
    }

    [Fact]
    public void When_CustomerCreatedValidatorConstructing_Then_RulesAreConfiguredCorrectly()
    {
        customerCreatedValidator.ShouldHaveRules(x => x.CustomerData.Name,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerCreated, string>>()
             .Create());

        customerCreatedValidator.ShouldHaveRules(x => x.CustomerData.Address,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerCreated, string>>()
             .Create());

        customerCreatedValidator.ShouldHaveRules(x => x.CustomerData.Email,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerCreated, string>>()
             .AddPropertyValidatorVerifier<AspNetCoreCompatibleEmailValidator<CustomerCreated>>()
             .Create());

        customerCreatedValidator.ShouldHaveRules(x => x.CustomerData.Phone,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerCreated, string>>()
             .AddPropertyValidatorVerifier<RegularExpressionValidator<CustomerCreated>>()
             .Create());
    }

    [Fact]
    public void When_CustomerValidatedValidatorConstructing_Then_4PropertiesShouldHaveRules()
    {
        customerValidatedValidator.ShouldHaveRulesCount(4);
    }

    [Fact]
    public void When_CustomerValidatedValidatorConstructing_Then_RulesAreConfiguredCorrectly()
    {
        customerValidatedValidator.ShouldHaveRules(x => x.CustomerCreated,
            BaseVerifiersSetComposer.Build()
                .AddChildValidatorVerifier<CustomerCreatedValidator, CustomerValidated, CustomerCreated>()
                .Create());

        customerValidatedValidator.ShouldHaveRules(x => x.CustomerCreated.EventId,
            BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerValidated, string>>()
                .Create());

        customerValidatedValidator.ShouldHaveRules(x => x.CustomerCreated.IsActive,
            BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerValidated, bool>>()
                .Create());

        customerValidatedValidator.ShouldHaveRules(x => x.CreatedAt,
            BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerValidated, DateTimeOffset>>()
                .AddPropertyValidatorVerifier<PredicateValidator<CustomerValidated, DateTimeOffset>>()
                .Create());
    }
}
