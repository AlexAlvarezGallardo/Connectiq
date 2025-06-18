namespace CustomerWorker.Domain.Queries;

public class CustomerQueryMapperProfile : Profile
{
    public CustomerQueryMapperProfile()
    {
        CreateMap<CustomerEntity, CustomerDetails>();

        CreateMap<CustomerEntity, CustomerDto>()
            .ForPath(dest => dest.Customer.Details, opt => opt.MapFrom(src => src))
            .ForPath(dest => dest.Customer.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss")));
    }
}
