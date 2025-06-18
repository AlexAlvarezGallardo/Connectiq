using Customers.Queries;

namespace Connectiq.API.InputType;

public class CustomerFiltersInputType : InputObjectType<CustomerFilters>
{
    protected override void Configure(IInputObjectTypeDescriptor<CustomerFilters> descriptor)
    {
        descriptor.Field(f => f.Name).Type<StringType>().DefaultValue(string.Empty);
        descriptor.Field(f => f.Address).Type<StringType>().DefaultValue(string.Empty);
        descriptor.Field(f => f.Email).Type<StringType>().DefaultValue(string.Empty);
        descriptor.Field(f => f.Phone).Type<StringType>().DefaultValue(string.Empty);
    }
}
