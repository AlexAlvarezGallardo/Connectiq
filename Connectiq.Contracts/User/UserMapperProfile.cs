using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;

namespace Connectiq.Contracts.User;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<UserEntity, GrpcUsers.User >()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ReverseMap();

        CreateMap<DateTimeOffset, Timestamp>()
            .ConvertUsing(src => Timestamp.FromDateTimeOffset(src));

        CreateMap<Timestamp, DateTimeOffset>()
            .ConvertUsing(src => src.ToDateTimeOffset());
    }
}
