using Connectiq.ProjectDefaults.Response.Query;
using Customers.Queries;

public class CustomerType : ObjectType<GetCustomerResponse>
{
    protected override void Configure(IObjectTypeDescriptor<GetCustomerResponse> descriptor)
    {
        descriptor.BindFields(BindingBehavior.Implicit);
    }
}

public class CustomerQueryResultType : ObjectType<QueryResponse<GetCustomerResponse>>
{
    protected override void Configure(IObjectTypeDescriptor<QueryResponse<GetCustomerResponse>> descriptor)
    {
        descriptor.Implements<InterfaceType<IQueryResponse<GetCustomerResponse>>>();

        descriptor.Field(f => f.Success).Type<NonNullType<BooleanType>>();
        descriptor.Field(f => f.Message).Type<StringType>();
        descriptor.Field(f => f.Data).Type<CustomerType>().Description("The validated customer data.");
    }
}

public class CustomersType : ObjectType<GetCustomersResponse>
{
    protected override void Configure(IObjectTypeDescriptor<GetCustomersResponse> descriptor)
    {
        descriptor.BindFields(BindingBehavior.Implicit);
    }
}

public class CustomersQueryResultType : ObjectType<QueryResponse<GetCustomersResponse>>
{
    protected override void Configure(IObjectTypeDescriptor<QueryResponse<GetCustomersResponse>> descriptor)
    {
        descriptor.Implements<InterfaceType<IQueryResponse<GetCustomersResponse>>>();

        descriptor.Field(f => f.Success).Type<NonNullType<BooleanType>>();
        descriptor.Field(f => f.Message).Type<StringType>();
        descriptor.Field(f => f.Data).Type<CustomersType>().Description("The customer data.");
    }
}
