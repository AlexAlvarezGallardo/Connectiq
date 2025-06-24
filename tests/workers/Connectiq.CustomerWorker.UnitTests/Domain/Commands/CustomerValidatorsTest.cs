using Customers;
using CustomerWorker.Domain.Commands;
using FluentValidation.Validators;
using FluentValidation.Validators.UnitTestExtension.Composer;
using FluentValidation.Validators.UnitTestExtension.Core;
using Xunit;

namespace Connectiq.CustomerWorker.UnitTests.Domain.Commands;

public class CustomerValidatorsTest
{
    readonly CustomerValidator customerValidator = new();
    readonly CustomerDetailsValidator customerDetailsValidator = new();

    [Fact]
    public void When_CustomerValidatorConstructing_Then_2PropertiesShouldHaveRules()
    {
        customerValidator.ShouldHaveRulesCount(2);
    }

    [Fact]
    public void When_CustomerValidatorConstructing_Then_RulesAreConfiguredCorrectly()
    {
        customerValidator.ShouldHaveRules(x => x.Details,
            BaseVerifiersSetComposer.Build()
                .AddChildValidatorVerifier<CustomerDetailsValidator, Customers.Customer, CustomerDetails>()
                .Create());

        customerValidator.ShouldHaveRules(x => x.Id,
            BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator<Customers.Customer, string>>()
                .Create());
    }

    [Fact]
    public void When_CustomerDetailsValidatorConstructing_Then_4PropertiesShouldHaveRules()
    {
        customerDetailsValidator.ShouldHaveRulesCount(4);
    }

    [Fact]
    public void When_CustomerDetailsValidatorConstructing_Then_RulesAreConfiguredCorrectly()
    {
        customerDetailsValidator.ShouldHaveRules(x => x.Name,
            BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerDetails, string>>()
                .Create());
        customerDetailsValidator.ShouldHaveRules(x => x.Address,
            BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerDetails, string>>()
                .Create());
        customerDetailsValidator.ShouldHaveRules(x => x.Email,
            BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerDetails, string>>()
                .AddPropertyValidatorVerifier<AspNetCoreCompatibleEmailValidator<CustomerDetails>>()
                .Create());
        customerDetailsValidator.ShouldHaveRules(x => x.Phone,
            BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator<CustomerDetails, string>>()
                .AddPropertyValidatorVerifier<RegularExpressionValidator<CustomerDetails>>()
                .Create());
    }

}
