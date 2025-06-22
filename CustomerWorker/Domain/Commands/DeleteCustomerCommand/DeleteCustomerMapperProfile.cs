namespace CustomerWorker.Domain.Commands.DeleteCustomerCommand;

public class DeleteCustomerMapperProfile : Profile
{
    public DeleteCustomerMapperProfile() 
    {
        CreateMap<DeleteCustomerInput, CustomerValidated>()
            .ForPath(dest => dest.Customer.Id, opt => opt.MapFrom(src => src.Id));

        CreateMap<CustomerValidated, DeleteCustomer>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Customer.Id));

        CreateMap<DeleteCustomer, CustomerEntity>();
    }
}
