namespace CustomerWorker.Domain.Commands;

public class GlobalCustomerMapperProfile : Profile
{
    public GlobalCustomerMapperProfile() 
    {
        CreateMap<CustomerDetails, Customers.Customer>()
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src));
    }
}