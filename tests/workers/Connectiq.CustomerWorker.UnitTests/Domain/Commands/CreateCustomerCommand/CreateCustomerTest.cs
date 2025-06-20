using CustomerWorker.Domain.Commands;
using CustomerWorker.Domain.Commands.CreateCustomerCommand;
using FluentValidation.Validators;
using FluentValidation.Validators.UnitTestExtension.Composer;
using FluentValidation.Validators.UnitTestExtension.Core;
using Xunit;

namespace Connectiq.CustomerWorker.UnitTests.Domain.Commands.CreateCustomerCommand;

public class CreateCustomerTest
{
    readonly CreateCustomerValidator _customerCreateValidator = new();

    [Fact]
    public void When_CustomerCreateValidatorConstructing_Then_3PropertiesShouldHaveRules()
    {
        _customerCreateValidator.ShouldHaveRulesCount(3);
    }

    [Fact]
    public void When_CustomerCreateValidatorConstructing_Then_RulesAreConfiguredCorrectly()
    {
        _customerCreateValidator.ShouldHaveRules(x => x.CustomerValidated,
            BaseVerifiersSetComposer.Build()
            .AddChildValidatorVerifier<CustomerValidatedValidator, CreateCustomer, CustomerValidated>()
            .Create());

        _customerCreateValidator.ShouldHaveRules(x => x.EventId,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotEmptyValidator<CreateCustomer, string>>()
             .Create());

        _customerCreateValidator.ShouldHaveRules(x => x.IsActive,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotEmptyValidator<CreateCustomer, bool>>()
             .Create());
    }
}