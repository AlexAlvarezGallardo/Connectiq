using AutoMapper;
using Connectiq.Contracts.User;
using Connectiq.GrpcUsers;
using Connectiq.Tests.Utilities;
using Connectiq.Tests.Utilities.Fixtures;
using FluentAssertions;
using Google.Protobuf;
using Xunit;

namespace Connectiq.Contracts.UnitTests.User;

public class UserMapperProfileTest(MapperFixture fixture) : IClassFixture<MapperFixture>
{
    private readonly IMapper _mapper = fixture.Mapper;
    readonly string _jsonDataEntity = typeof(GrpcUsers.User).Name;

    [Fact]
    public void Should_Map_CreateUserInput_To_UserValidated()
    {
        var inputPath = JsonDataLoader.GetDataPath(_jsonDataEntity, "User.json");
        var user = GrpcUsers.User.Parser.ParseJson(File.ReadAllText(inputPath));
        var input = _mapper.Map<CreateUserInput>(user);
        var result = _mapper.Map<UserValidated>(input);

        result.CreateUserInput.User.Username.Should().Be("Alex");
        result.CreateUserInput.User.Password.Should().Be("a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6");
        var expectedRoles = new List<Role> { Role.Admin, Role.User };
        result.CreateUserInput.User.Roles.Should().BeEquivalentTo(expectedRoles);
        result.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Map_CreateUserInput_To_UserNotValidated()
    {
        var inputPath = JsonDataLoader.GetDataPath(_jsonDataEntity, "User.json");
        var user = GrpcUsers.User.Parser.ParseJson(File.ReadAllText(inputPath));
        var input = _mapper.Map<CreateUserInput>(user);
        var result = _mapper.Map<UserNotValidated>(input);

        result.CreateUserInput.User.Username.Should().Be("Alex");
        result.CreateUserInput.User.Password.Should().Be("a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6");
        var expectedRoles = new List<Role> { Role.Admin, Role.User };
        result.CreateUserInput.User.Roles.Should().BeEquivalentTo(expectedRoles);
        result.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Map_UserValidated_To_CreateUser()
    {
        var inputPath = JsonDataLoader.GetDataPath(_jsonDataEntity, "UserValidated.json");
        var createUser = JsonDataLoader.LoadFromFile<UserValidated>(inputPath);

        var result = _mapper.Map<CreateUser>(createUser);

        result.UserValidated.CreateUserInput.User.Username.Should().Be("Alex");
        result.UserValidated.CreateUserInput.User.Password.Should().Be("otraHashDePasswordXYZ");
        result.UserValidated.CreatedAt.Should().Be(DateTimeOffset.Parse("2025-06-10T16:01:09.1234567+02:00"));
        result.UserValidated.IsValid.Should().BeTrue();
        result.IsActive.Should().BeTrue();
    }
}
