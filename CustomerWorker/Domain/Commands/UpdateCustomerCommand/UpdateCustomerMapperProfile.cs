namespace CustomerWorker.Domain.Commands.UpdateCustomerCommand;

public class UpdateCustomerMapperProfile : Profile
{
    public UpdateCustomerMapperProfile() 
    {
        CreateMap<CustomerValidated, UpdateCustomer>()
               .IncludeBase<CustomerValidated, CustomerBase>();

        CreateMap<UpdateCustomerInput, CustomerValidated>()
          .ForPath(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer.Details))
          .ForPath(dest => dest.Customer.Id, opt => opt.MapFrom(src => src.Customer.Id))
          .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
          .ForMember(dest => dest.IsValid, opt => opt.MapFrom(_ => true));
    }
}
