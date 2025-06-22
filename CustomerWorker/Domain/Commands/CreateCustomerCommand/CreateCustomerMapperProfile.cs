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
            .IncludeBase<CustomerValidated, CustomerBase>();
    }
}
