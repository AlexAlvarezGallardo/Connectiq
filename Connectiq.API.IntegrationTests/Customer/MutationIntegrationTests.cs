using Connectiq.API.IntegrationTests.Tests;
using ConnectiqApiNS;
using FluentAssertions;

namespace Connectiq.API.IntegrationTests.Customer;

public class MutationIntegrationTests(ApiFixture _fixture) : IClassFixture<ApiFixture>
{
    [Theory]
    [InlineData("Alex Álvarez", "alex@example.com", "+34 600 123 456", "test")]
    [InlineData("María López", "maria@example.com", "+34 611 987 654", "fake street 123")]
    [InlineData("Juan Pérez", "juan@example.com", "+34 699 000 111", "real avennue 45")]
    public async Task CreateClient_Should_Return_Success_With_Valid_Data(
        string name,
        string email,
        string phone,
        string address)
    {
        var graphqlClient = await _fixture.CreateConnectiqApiClientAsync();

        var input = new CreateCustomerInput
        {
            Details = new CustomerDetailsInput
            {
                Name = name,
                Email = email,
                Phone = phone,
                Address = address
            }
        };

        var result = await graphqlClient.CreateCustomer.ExecuteAsync(input);

        result.Should().NotBeNull();
        result.Errors.Should().BeEmpty("because the creation should succeed");
        result.Data.Should().NotBeNull();

        var customer = result.Data.CreateCustomer?.Data?.Customer;
        customer.Should().NotBeNull("because the customer must be created");

        customer!.Id.Should().BeEmpty();
        customer.Details!.Name.Should().Be(name);
        customer.Details.Email.Should().Be(email);
        customer.Details.Phone.Should().Be(phone);
        customer.Details.Address.Should().Be(address);
    }

    [Theory]
    [InlineData("", "alex@example.com", "+34 600 123 456", "Address Test", "Details.Name", "NotEmptyValidator", "El nombre es obligatorio")]
    [InlineData("Alex Álvarez", "alex@example.com", "+34 600 123 456", "", "Details.Address", "NotEmptyValidator", "La dirección es obligatoria")]
    [InlineData("Alex Álvarez", "", "+34 600 123 456", "Address Test", "Details.Email", "NotEmptyValidator", "El email es obligatorio")]
    [InlineData("Alex Álvarez", "alex", "+34 600 123 456", "Address Test", "Details.Email", "EmailValidator", "Formato de email inválido")]
    [InlineData("Alex Álvarez", "alex@example.com", "", "Address Test", "Details.Phone", "NotEmptyValidator", "El teléfono es obligatorio")]
    [InlineData("Alex Álvarez", "alex@example.com", "600 123 456", "Address Test", "Details.Phone", "RegularExpressionValidator", "Formato de teléfono inválido (+34 600 123 456)")]
    public async Task CreateClient_WithInvalidFields_Should_Return_ValidationErrors(
        string name,
        string email,
        string phone,
        string address,
        string expectedProperty,
        string validator,
        string expectedErrorMessage)
    {
        var graphqlClient = await _fixture.CreateConnectiqApiClientAsync();

        var input = new CreateCustomerInput
        {
            Details = new CustomerDetailsInput
            {
                Name = name,
                Email = email,
                Phone = phone,
                Address = address
            }
        };

        var result = await graphqlClient.CreateCustomer.ExecuteAsync(input);

        result.Should().NotBeNull();

        var createCustomer = result.Data!.CreateCustomer!;
        createCustomer.Should().BeAssignableTo<ICreateCustomer_CreateCustomer_MutationResponseOfCustomerValidated>();

        var createdCustomer = (ICreateCustomer_CreateCustomer_MutationResponseOfCustomerValidated)createCustomer!;

        createdCustomer.Success.Should().BeFalse("because the input is invalid");
        createdCustomer.StatusCode.Should().Be(ConnectiqApiNS.HttpStatusCode.BadRequest);
        createdCustomer.Message.Should().Be("Validation Error");

        createdCustomer.Errors.Should().NotBeNullOrEmpty("because validation errors are expected");

        var error = createdCustomer.Errors!.FirstOrDefault(e =>
            e.PropertyName == expectedProperty &&
            e.ErrorMessage == expectedErrorMessage &&
            e.ErrorCode == validator);

        error.Should().NotBeNull($"because an error on '{expectedProperty}' is expected");

        createdCustomer.Data!.IsValid.Should().BeFalse("because the validation failed");
        createdCustomer.Data.Customer.Should().NotBeNull();
        createdCustomer.Data.Customer.Details!.Address.Should().Be(address);
    }

    [Fact]
    public async Task UpdateCustomer_Should_Return_Success()
    {
        var graphqlClient = await _fixture.CreateConnectiqApiClientAsync();

        var input = new GetAllFiltersInput { PageSize = 1, Page = 1 };

        var customersResult = await graphqlClient!.GetAllCustomers.ExecuteAsync(input);
        customersResult.Should().NotBeNull();
        customersResult.Data.Should().NotBeNull();

        var customerId = customersResult.Data!.AllCustomers!.Data!.Customers![0]!.Customer!.Id;
        customerId.Should().NotBeNullOrWhiteSpace();

        var updateInput = new UpdateCustomerInput
        {
            Customer = new CustomerInput
            {
                Id = customerId,
                Details = new CustomerDetailsInput
                {
                    Name = "Update Alex",
                    Email = "alex.updated@example.com",
                    Phone = "+34 699 999 999",
                    Address = "New address updated"
                }
            }
        };

        var result = await graphqlClient.UpdateCustomer.ExecuteAsync(updateInput);
        var customerUpdate = result!.Data.UpdateCustomer;

        customerUpdate.Should().BeAssignableTo<IUpdateCustomer_UpdateCustomer_MutationResponseOfCustomerValidated>();
        var updatedCustomer = (IUpdateCustomer_UpdateCustomer_MutationResponseOfCustomerValidated)customerUpdate!;

        updatedCustomer.Should().NotBeNull();
        updatedCustomer.Errors.Should().BeEmpty("because update should succeed");
        updatedCustomer.Success.Should().BeTrue();
        updatedCustomer.StatusCode.Should().Be(ConnectiqApiNS.HttpStatusCode.Ok);

        updatedCustomer.Should().BeAssignableTo<IUpdateCustomer_UpdateCustomer_MutationResponseOfCustomerValidated>();

        updatedCustomer.Data.Should().NotBeNull();
        updatedCustomer.Data.Customer.Should().NotBeNull();
        updatedCustomer.Data.Customer.Id.Should().Be(customerId);
        updatedCustomer.Data.Customer.Details!.Name.Should().Be("Update Alex");
        updatedCustomer.Data.Customer.Details.Email.Should().Be("alex.updated@example.com");
        updatedCustomer.Data.Customer.Details.Phone.Should().Be("+34 699 999 999");
        updatedCustomer.Data.Customer.Details.Address.Should().Be("New address updated");
    }

    [Theory]
    [InlineData("", "alex@example.com", "+34 600 123 456", "Address Test", "Customer.Details.Name", "NotEmptyValidator", "El nombre es obligatorio")]
    [InlineData("Alex Álvarez", "alex@example.com", "+34 600 123 456", "", "Customer.Details.Address", "NotEmptyValidator", "La dirección es obligatoria")]
    [InlineData("Alex Álvarez", "alex", "+34 600 123 456", "Address Test", "Customer.Details.Email", "EmailValidator", "Formato de email inválido")]
     public async Task UpdateCustomer_WithInvalidFields_Should_Return_ValidationErrors(
        string name,
        string email,
        string phone,
        string address,
        string expectedProperty,
        string validator,
        string expectedErrorMessage)
     {
        var graphqlClient = await _fixture.CreateConnectiqApiClientAsync();

        var getAllInput = new GetAllFiltersInput { Page = 1, PageSize = 1 };
        var customersResult = await graphqlClient.GetAllCustomers.ExecuteAsync(getAllInput);
        var customerId = customersResult?.Data?.AllCustomers?.Data?.Customers?.FirstOrDefault()?.Customer?.Id;
        customerId.Should().NotBeNullOrWhiteSpace("se necesita un ID de cliente válido para el update");

        var updateInput = new UpdateCustomerInput
        {
            Customer = new CustomerInput
            {
                Id = customerId!,
                Details = new CustomerDetailsInput
                {
                    Name = name,
                    Email = email,
                    Phone = phone,
                    Address = address
                }
            }
        };

        var result = await graphqlClient.UpdateCustomer.ExecuteAsync(updateInput);

        result.Should().NotBeNull();
        var response = result.Data!.UpdateCustomer;

        response.Should().BeAssignableTo<IUpdateCustomer_UpdateCustomer_MutationResponseOfCustomerValidated>();
        var updateResponse = (IUpdateCustomer_UpdateCustomer_MutationResponseOfCustomerValidated)response!;

        updateResponse.Success.Should().BeFalse("porque la entrada no es válida");
        updateResponse.StatusCode.Should().Be(ConnectiqApiNS.HttpStatusCode.BadRequest);
        updateResponse.Message.Should().Be("Validation Error");

        updateResponse.Errors.Should().NotBeNullOrEmpty();
        updateResponse.Errors!.Count().Should().Be(1);

        var error = updateResponse.Errors.FirstOrDefault(e =>
            e.PropertyName == expectedProperty &&
            e.ErrorMessage == expectedErrorMessage &&
            e.ErrorCode == validator);

        error.Should().NotBeNull($"porque se esperaba un error de validación en '{expectedProperty}'");
     }

    [Fact]
    public async Task SoftDeleteCustomer_Should_Return_Success()
    {
        var graphqlClient = await _fixture.CreateConnectiqApiClientAsync();

        var getAllInput = new GetAllFiltersInput { Page = 1, PageSize = 1 };

        var getCustomersResult = await graphqlClient.GetAllCustomers.ExecuteAsync(getAllInput);

        getCustomersResult.Should().NotBeNull();
        getCustomersResult.Data.Should().NotBeNull();
        var customerId = getCustomersResult.Data.AllCustomers!.Data!.Customers![0]!.Customer!.Id;
        customerId.Should().NotBeNullOrWhiteSpace("porque necesitamos un ID válido para hacer el soft delete");

        var softDeleteInput = new SoftDeleteCustomerInput { Id = customerId };

        var deleteResult = await graphqlClient.SoftDeleteCustomer.ExecuteAsync(softDeleteInput);

        deleteResult.Should().NotBeNull();
        deleteResult.Errors.Should().BeEmpty("porque se espera que el borrado lógico sea exitoso");
        deleteResult.Data.Should().NotBeNull();

        var deleteResponse = deleteResult.Data!.SoftDelete!;
        deleteResponse.Should().BeAssignableTo<ISoftDeleteCustomer_SoftDelete_MutationResponseOfCustomerValidated>();
        var softDeleteCustomer = (ISoftDeleteCustomer_SoftDelete_MutationResponseOfCustomerValidated)deleteResponse!;

        softDeleteCustomer.Success.Should().BeTrue();
        softDeleteCustomer.StatusCode.Should().Be(ConnectiqApiNS.HttpStatusCode.Ok);
        softDeleteCustomer.Message.Should().BeNullOrEmpty(); 

        softDeleteCustomer.Data.Should().NotBeNull();
        softDeleteCustomer.Data.Customer.Should().NotBeNull();
        softDeleteCustomer.Data.Customer.Id.Should().Be(customerId);
    }

    [Theory]
    [InlineData("", "Id", "NotEmptyValidator")] 
    public async Task SoftDeleteCustomer_WithInvalidId_Should_Return_ValidationError(
        string invalidId,
        string propertyName,
        string errorCode)
    {
        var graphqlClient = await _fixture.CreateConnectiqApiClientAsync();

        var input = new SoftDeleteCustomerInput { Id = invalidId };

        var result = await graphqlClient.SoftDeleteCustomer.ExecuteAsync(input);

        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();

        var response = result.Data!.SoftDelete!;
        response.Should().BeAssignableTo<ISoftDeleteCustomer_SoftDelete_MutationResponseOfCustomerValidated>();
        var softDeleteCustomer = (ISoftDeleteCustomer_SoftDelete_MutationResponseOfCustomerValidated)response!;

        softDeleteCustomer.Should().NotBeNull();
        softDeleteCustomer.Success.Should().BeFalse("porque el id es inválido");
        softDeleteCustomer.StatusCode.Should().Be(ConnectiqApiNS.HttpStatusCode.BadRequest);
        softDeleteCustomer.Message.Should().Be("Validation Error");

        softDeleteCustomer.Errors.Should().NotBeNullOrEmpty("porque se esperaba al menos un error de validación");

        var idError = softDeleteCustomer.Errors!.FirstOrDefault(e =>
            e.PropertyName == propertyName &&
            e.ErrorCode == errorCode);

        idError.Should().NotBeNull("porque se esperaba un error de validación sobre el campo Id");
        idError!.ErrorMessage.Should().Be("El Id del customer es obligatorio"); 

        softDeleteCustomer.Data.Should().NotBeNull();
        softDeleteCustomer.Data.Customer.Should().NotBeNull();
        softDeleteCustomer.Data.Customer.Id.Should().BeEmpty("porque el ID era inválido y no debe retornar un ID válido");
    }
}