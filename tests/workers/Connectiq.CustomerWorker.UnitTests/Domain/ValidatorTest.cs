using CustomerWorker.Domain;
using CustomerWorker.Domain.Commands;
using FluentValidation.Validators;
using FluentValidation.Validators.UnitTestExtension.Composer;
using FluentValidation.Validators.UnitTestExtension.Core;
using Xunit;

namespace Connectiq.CustomerWorker.UnitTests.Domain;

public class ValidatorTest
{
    readonly CustomerCreateValidator customerCreateValidator = new();
    readonly CustomerValidatedValidator customerValidatedValidator = new();

    [Fact]
    public void When_CustomerCreateValidatorConstructing_Then_3PropertiesShouldHaveRules()
    {
        customerCreateValidator.ShouldHaveRulesCount(3);
    }

    [Fact]
    public void When_CustomerCreateValidatorConstructing_Then_RulesAreConfiguredCorrectly()
    {
        customerCreateValidator.ShouldHaveRules(x => x.CustomerValidated,
            BaseVerifiersSetComposer.Build()
                .AddChildValidatorVerifier<CustomerValidatedValidator, CustomerCreate, CustomerValidated>()
                .Create());

        customerCreateValidator.ShouldHaveRules(x => x.EventId,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerCreate, string>>()
             .Create());

        customerCreateValidator.ShouldHaveRules(x => x.IsActive,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerCreate, bool>>()
             .Create());
    }

    [Fact]
    public void When_CustomerValidatedValidatorConstructing_Then_6PropertiesShouldHaveRules()
    {
        customerValidatedValidator.ShouldHaveRulesCount(6);
    }

    [Fact]
    public void When_CustomerValidatedValidatorConstructing_Then_RulesAreConfiguredCorrectly()
    {
        customerValidatedValidator.ShouldHaveRules(x => x.Customer.Details.Name,
            BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerValidated, string>>()
                .Create());

        customerValidatedValidator.ShouldHaveRules(x => x.Customer.Details.Address,
            BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerValidated, string>>()
                .Create());

        customerValidatedValidator.ShouldHaveRules(x => x.Customer.Details.Email,
            BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerValidated, string>>()
                .AddPropertyValidatorVerifier<AspNetCoreCompatibleEmailValidator<CustomerValidated>>()
                .Create());

        customerValidatedValidator.ShouldHaveRules(x => x.Customer.Details.Phone,
            BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerValidated, string>>()
                .AddPropertyValidatorVerifier<RegularExpressionValidator<CustomerValidated>>()
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
