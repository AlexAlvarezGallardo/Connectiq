using Customers.Queries;

namespace Connectiq.API.GraphQL.Types.Customer;

public class CustomerCommandType : ObjectType<CustomerValidated>
{
    protected override void Configure(IObjectTypeDescriptor<CustomerValidated> descriptor)
    {
        descriptor.BindFields(BindingBehavior.Implicit);
    }
}

public class CustomerValidatedResultType : ObjectType<MutationResponse<CustomerValidated>>
{
    protected override void Configure(IObjectTypeDescriptor<MutationResponse<CustomerValidated>> descriptor)
    {
        descriptor.Implements<InterfaceType<IMutationResponse<CustomerValidated>>>();

        descriptor.Field(f => f.Success).Type<NonNullType<BooleanType>>();
        descriptor.Field(f => f.Message).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.Data).Type<CustomerCommandType>().Description("The validated customer data.");
    }
}