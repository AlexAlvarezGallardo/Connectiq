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
    readonly UserValidator userValidator = new();

    [Fact]
    public void When_UserValidatorConstructing_Then_5PropertiesShouldHaveRules()
    {
        userValidator.ShouldHaveRulesCount(5);
    }

    [Fact]
    public void When_UserValidatorConstructing_Then_RulesAreConfiguredCorrectly()
    {
        userValidator.ShouldHaveRules(x => x.Username,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotEmptyValidator<GrpcUsers.User, string>>()
             .AddPropertyValidatorVerifier<LengthValidator<GrpcUsers.User>>(3, 50)
             .Create());

        userValidator.ShouldHaveRules(x => x.PasswordHash,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotEmptyValidator<GrpcUsers.User, string>>()
             .Create());

        userValidator.ShouldHaveRules(x => x.Email,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotEmptyValidator<GrpcUsers.User, string>>()
             .AddPropertyValidatorVerifier<AspNetCoreCompatibleEmailValidator<GrpcUsers.User>>()
             .Create());

        userValidator.ShouldHaveRules(x => x.Roles,
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<NotNullValidator<GrpcUsers.User, RepeatedField<Role>>>()
             .Create());

        userValidator.ShouldHaveRules(x => x.CreatedAt.ToDateTimeOffset(),
            BaseVerifiersSetComposer.Build()
             .AddPropertyValidatorVerifier<LessThanOrEqualValidator<GrpcUsers.User, DateTimeOffset>>()
             .Create());
    }
}
