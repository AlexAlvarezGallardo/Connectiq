using AutoMapper;
using Google.Protobuf.WellKnownTypes;

namespace Connectiq.Contracts.User;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<DateTimeOffset, Timestamp>()
            .ConvertUsing(src => Timestamp.FromDateTimeOffset(src));

        CreateMap<Timestamp, DateTimeOffset>()
            .ConvertUsing(src => src.ToDateTimeOffset());

        //This mapping is used to convert the gRPC User object to CreateUserInput
        //just for TESTING
        CreateMap<GrpcUsers.User, CreateUserInput>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));

        CreateMap<CreateUserInput, UserValidated>()
            .ForMember(dest => dest.CreateUserInput, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.IsValid, opt => opt.MapFrom(src => true)); 

        CreateMap<CreateUserInput, UserNotValidated>()
            .ForMember(dest => dest.CreateUserInput, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.IsValid, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.NotValidatedMessage, opt => opt.Ignore());

        CreateMap<UserValidated, CreateUser>()
            .ForMember(dest => dest.UserValidated, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.EventId, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

        CreateMap<CreateUser, UserEntity>();
    }
}