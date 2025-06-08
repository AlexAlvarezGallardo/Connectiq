using AutoMapper;
using Connectiq.Contracts.User;
using Connectiq.GrpcUsers;
using Connectiq.Tests.Utilities;
using Connectiq.Tests.Utilities.Fixtures;
using FluentAssertions;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System.IO;
using Xunit;

namespace Connectiq.Contracts.UnitTests.User;

public class UserMapperProfileTest(MapperFixture fixture) : IClassFixture<MapperFixture>
{
    private readonly IMapper _mapper = fixture.Mapper;
    readonly string _jsonDataEntity = typeof(GrpcUsers.User).Name;

    [Fact]
    public void Should_Map_User_To_UserEntity()
    {
        var inputPath = JsonDataLoader.GetDataPath(_jsonDataEntity, "User.json");
        var json = File.ReadAllText(inputPath);
        var input = JsonParser.Default.Parse<GrpcUsers.User>(json);
        var result = _mapper.Map<UserEntity>(input);

        result.Id.Should().Be(Guid.Parse("6fa459ea-ee8a-3ca4-894e-db77e160355e"));
        result.Username.Should().Be("alex");
        result.PasswordHash.Should().Be("hashedPassword123");
        result.Email.Should().Be("alex.doe@example.com");
        result.CreatedAt.Should().Be(DateTimeOffset.Parse("2025-06-08T14:30:00Z"));
        result.IsActive.Should().BeTrue();
        var expectedRoles = new List<Role> { Role.Admin, Role.User };
        result.Roles.Should().BeEquivalentTo(expectedRoles);
    }

    [Fact]
    public void Should_Map_UserEntity_To_User()
    {
        var inputPath = JsonDataLoader.GetDataPath(_jsonDataEntity, "UserEntity.json");
        var userEntity = JsonDataLoader.LoadFromFile<UserEntity>(inputPath);

        var result = _mapper.Map<GrpcUsers.User>(userEntity);

        result.Id.Should().Be("4f6d9a88-3f32-4d48-b2a8-5f4db9d43f1c");
        result.Username.Should().Be("anotheruser");
        result.PasswordHash.Should().Be("securehash");
        result.Email.Should().Be("another@example.com");
        
        var expectedTimestamp = Timestamp.FromDateTime(DateTime.Parse("2025-06-06T12:00:00Z").ToUniversalTime());
        result.CreatedAt.Should().Be(expectedTimestamp);
        result.IsActive.Should().BeFalse();

        var expectedRoles = new[] { Role.User, Role.SuperAdmin };
        result.Roles.Should().BeEquivalentTo(expectedRoles);
    }
}
