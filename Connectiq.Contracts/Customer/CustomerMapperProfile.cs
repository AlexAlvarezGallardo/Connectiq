using AutoMapper;

namespace Connectiq.Contracts.Customer;

public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<CreateCustomerInput, CustomerCreated>()
            .ForMember(dest => dest.CustomerData, opt => opt.MapFrom(src => src.Customer));

        CreateMap<CustomerCreated, CustomerValidated>()
            .ForMember(dest => dest.CustomerCreated, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.IsValid, opt => opt.MapFrom(_ => true));

        CreateMap<CustomerCreated, CustomerNotValidated>()
            .ForMember(dest => dest.CustomerCreated, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsValid, opt => opt.Ignore())
            .ForMember(dest => dest.NotValidatedMessage, opt => opt.Ignore());

        CreateMap<CustomerValidated, CustomerEntity>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CustomerCreated.CustomerData.Name))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.CustomerCreated.CustomerData.Address))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.CustomerCreated.CustomerData.Phone))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.CustomerCreated.CustomerData.Email))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.CustomerCreated.IsActive))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
