using Connectiq.Contracts.Customer;
using Connectiq.Contracts.User;
using Connectiq.GrpcUsers;
using FluentValidation.Validators;
using FluentValidation.Validators.UnitTestExtension.Composer;
using FluentValidation.Validators.UnitTestExtension.Core;
using Google.Protobuf.Collections;
using Xunit;

namespace Connectiq.Contracts.UnitTests.User;

public class ValidatorTest
{
    readonly CreateUserInputValidator _createUserInputValidator = new();
    readonly CreateUserValidator _createUserValidator = new();

    [Fact]
    public void When_CreateUserInputValidatorConstructing_Then_3PropertiesShouldHaveRules()
    {
        _createUserInputValidator.ShouldHaveRulesCount(3);
    }

    [Fact]
    public void When_CreateUserInputValidatorConstructing_Then_RulesAreConfiguredCorrectly()
    {
        _createUserInputValidator.ShouldHaveRules(x => x.User.Username,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotEmptyValidator<CreateUserInput, string>>()
             .AddPropertyValidatorVerifier<LengthValidator<CreateUserInput>>(3, 50)
             .Create());

        _createUserInputValidator.ShouldHaveRules(x => x.User.Password,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotEmptyValidator<CreateUserInput, string>>()
             .Create());

        _createUserInputValidator.ShouldHaveRules(x => x.User.Roles,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotNullValidator<CreateUserInput, RepeatedField<Role>>>()
             .Create());
    }

    [Fact]
    public void When_CreateUserValidatorConstructing_Then_4PropertiesShouldHaveRules()
    {
        _createUserValidator.ShouldHaveRulesCount(4);
    }

    [Fact]
    public void When_CreateUserValidatorConstructing_Then_RulesAreConfiguredCorrectly()
    {
        _createUserValidator.ShouldHaveRules(x => x.EventId,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotEmptyValidator<CreateUser, string>>()
             .Create());

        _createUserValidator.ShouldHaveRules(x => x.IsActive,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotNullValidator<CreateUser, bool>>()
             .Create());

        _createUserValidator.ShouldHaveRules(x => x.UserValidated.CreatedAt,
            BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotNullValidator<CreateUser, DateTimeOffset>>()
                .AddPropertyValidatorVerifier<LessThanOrEqualValidator<CreateUser, DateTimeOffset>>()
                .Create());
    }
}
