namespace CustomerWorker.Domain.Commands.SoftDeleteCustomerCommand;

public class SoftDeleteCustomerMapperProfile : Profile
{
    public SoftDeleteCustomerMapperProfile()
    {
        CreateMap<SoftDeleteCustomerInput, CustomerValidated>()
            .ForPath(dest => dest.Customer.Id, opt => opt.MapFrom(src => src.Id));

        CreateMap<CustomerValidated, SoftDeleteCustomer>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Customer.Id));

        CreateMap<SoftDeleteCustomer, CustomerEntity>();
    }
}
