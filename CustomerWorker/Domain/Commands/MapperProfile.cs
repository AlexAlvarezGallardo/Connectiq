namespace CustomerWorker.Domain.Commands;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<CreateCustomerInput, CustomerValidated>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.IsValid, opt => opt.MapFrom(_ => true));

        CreateMap<CreateCustomerInput, CustomerNotValidated>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.IsValid, opt => opt.MapFrom(_ => false));

        CreateMap<CustomerValidated, CustomerCreate>()
            .ForMember(dest => dest.CustomerValidated, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));

        CreateMap<CustomerCreate, CustomerEntity>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CustomerValidated.Customer.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.CustomerValidated.Customer.Email))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.CustomerValidated.Customer.Address))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.CustomerValidated.Customer.Phone))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CustomerValidated.CreatedAt));
    }
}
