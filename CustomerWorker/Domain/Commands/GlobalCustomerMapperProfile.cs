namespace CustomerWorker.Domain.Commands;

public class GlobalCustomerMapperProfile : Profile
{
    public GlobalCustomerMapperProfile()
    {
        CreateMap<CustomerDetails, Customers.Customer>()
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src));

        CreateMap<CustomerValidated, CustomerBase>()
            .ForMember(dest => dest.CustomerValidated, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));

        CreateMap<CustomerBase, CustomerEntity>()
            .IncludeAllDerived()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CustomerValidated.Customer.Details.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.CustomerValidated.Customer.Details.Email))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.CustomerValidated.Customer.Details.Address))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.CustomerValidated.Customer.Details.Phone))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => SafeParseGuid(src.CustomerValidated.Customer.Id)))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CustomerValidated.CreatedAt));
    }

    static Guid SafeParseGuid(string input)
        => Guid.TryParse(input, out Guid guid) ? guid : Guid.Empty;
}