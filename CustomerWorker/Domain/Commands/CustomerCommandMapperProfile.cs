namespace CustomerWorker.Domain.Commands;

public class CustomerCommandMapperProfile : Profile
{
    public CustomerCommandMapperProfile()
    {
        CreateMap<CustomerDetails, Customers.Customer>()
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src));

        CreateMap<CreateCustomerInput, CustomerValidated>()
            .ForPath(dest => dest.Customer, opt => opt.MapFrom(src => src.Details))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.IsValid, opt => opt.MapFrom(_ => true));

        CreateMap<CreateCustomerInput, CustomerNotValidated>()
            .ForPath(dest => dest.Customer, opt => opt.MapFrom(src => src.Details))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.IsValid, opt => opt.MapFrom(_ => false));

        CreateMap<CustomerValidated, CustomerCreate>()
            .ForMember(dest => dest.CustomerValidated, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));

        CreateMap<CustomerCreate, CustomerEntity>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CustomerValidated.Customer.Details.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.CustomerValidated.Customer.Details.Email))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.CustomerValidated.Customer.Details.Address))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.CustomerValidated.Customer.Details.Phone))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CustomerValidated.CreatedAt));
    }
}
