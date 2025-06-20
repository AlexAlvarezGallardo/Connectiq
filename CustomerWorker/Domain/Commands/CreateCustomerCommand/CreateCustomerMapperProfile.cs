namespace CustomerWorker.Domain.Commands.CreateCustomerCommand;

public class CreateCustomerMapperProfile : Profile
{
    public CreateCustomerMapperProfile()
    {
        CreateMap<CreateCustomerInput, CustomerValidated>()
            .ForPath(dest => dest.Customer, opt => opt.MapFrom(src => src.Details))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.IsValid, opt => opt.MapFrom(_ => true));

        CreateMap<CustomerValidated, CreateCustomer>()
            .ForMember(dest => dest.CustomerValidated, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));

        CreateMap<CreateCustomer, CustomerEntity>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CustomerValidated.Customer.Details.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.CustomerValidated.Customer.Details.Email))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.CustomerValidated.Customer.Details.Address))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.CustomerValidated.Customer.Details.Phone))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CustomerValidated.CreatedAt));
    }
}
