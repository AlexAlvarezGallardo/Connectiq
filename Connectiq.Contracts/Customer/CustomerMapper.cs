using Riok.Mapperly.Abstractions;

namespace Connectiq.Contracts.Customer;

[Mapper]
public partial class CustomerMapper
{
    [MapperIgnoreTarget(nameof(CustomerCreated.Id))]
    public partial CustomerCreated ToEvent(CreateCustomerInput customerInput);
}
