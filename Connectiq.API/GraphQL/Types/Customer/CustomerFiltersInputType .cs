using Customers.Queries;

namespace Connectiq.API.GraphQL.Types.Customer;

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