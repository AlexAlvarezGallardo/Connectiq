using Connectiq.API.IntegrationTests.Tests;
using ConnectiqApiNS;
using FluentAssertions;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace Connectiq.API.IntegrationTests;

public class QueryIntegrationTests(ApiFixture _fixture) : IClassFixture<ApiFixture> 
{
    [Fact]
    public async Task Graphql_Endpoint_Should_Return_Success() 
    {
        var client = await _fixture.CreateHttpClientAsync("connectiq-api");

        var response = await client.GetAsync("graphql");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(1, 1000, 1000)]
    [InlineData(1, 500, 500)]  
    [InlineData(2, 1500, 500)] 
    public async Task GetAllCustomers_Should_Return_ExpectedTotalCount(
        int page,
        int pageSize,
        int expectedTotalCount)
    {
        var graphqlClient = await _fixture.CreateConnectiqApiClientAsync();

        var input = new GetAllFiltersInput
        {
            Page = page,
            PageSize = pageSize,
        };

        var result = await graphqlClient.GetAllCustomers.ExecuteAsync(input);

        result.Should().NotBeNull();
        result.Errors.Should().BeNullOrEmpty("because the request should succeed");

        var customers = result!.Data!.AllCustomers as IGetAllCustomers_AllCustomers_QueryResponseOfGetCustomersResponse;
        customers.Should().NotBeNull("because we expect a valid response");

        customers!.Data!.TotalCount.Should().Be(expectedTotalCount);
        customers.Success.Should().BeTrue();
        customers.StatusCode.Should().Be(ConnectiqApiNS.HttpStatusCode.Ok);
    }


    [Fact]
    public async Task GetCustomerById_Should_Return_Success()
    {
        var graphqlClient = await _fixture.CreateConnectiqApiClientAsync();
        graphqlClient.Should().NotBeNull();

        var input = new GetAllFiltersInput
        {
            PageSize = 1,
            Page = 1
        };

        var customersResult = await graphqlClient!.GetAllCustomers.ExecuteAsync(input);
        customersResult.Should().NotBeNull();
        customersResult.Data.Should().NotBeNull();

        var allCustomers = customersResult.Data!.AllCustomers;
        allCustomers.Should().BeAssignableTo<IGetAllCustomers_AllCustomers_QueryResponseOfGetCustomersResponse>();

        var customers = (IGetAllCustomers_AllCustomers_QueryResponseOfGetCustomersResponse)allCustomers!;
        customers.Success.Should().BeTrue();
        customers.StatusCode.Should().Be(ConnectiqApiNS.HttpStatusCode.Ok);
        customers.Data.Should().NotBeNull();
        customers.Data!.Customers.Should().NotBeNullOrEmpty();

        var expected = customers.Data!.Customers!.First();
        expected!.Customer.Should().NotBeNull();
        expected.Customer!.Details.Should().NotBeNull();

        var id = expected.Customer.Id;
        id.Should().NotBeNullOrWhiteSpace();

        var result = await graphqlClient.GetCustomerById.ExecuteAsync(id!);
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();

        var customerById = result.Data!.CustomerById;
        customerById.Should().BeAssignableTo<IGetCustomerById_CustomerById_QueryResponseOfGetCustomerResponse>();

        var customer = (IGetCustomerById_CustomerById_QueryResponseOfGetCustomerResponse)customerById!;
        customer.Data.Should().NotBeNull();
        customer.Data!.CustomerDto.Should().NotBeNull();
        customer.Data.CustomerDto!.Customer.Should().NotBeNull();
        customer.Data.CustomerDto.Customer!.Details.Should().NotBeNull();

        var actual = customer.Data.CustomerDto;

        actual.CreatedAt.Should().Be(expected.CreatedAt);
        actual.EventId.Should().Be(expected.EventId);
        actual.IsActive.Should().Be(expected.IsActive);
        actual.Customer!.Id.Should().Be(expected.Customer.Id);
        actual.Customer.Details!.Name.Should().Be(expected.Customer.Details!.Name);
        actual.Customer.Details.Address.Should().Be(expected.Customer.Details.Address);
        actual.Customer.Details.Email.Should().Be(expected.Customer.Details.Email);
        actual.Customer.Details.Phone.Should().Be(expected.Customer.Details.Phone);

        customer.Success.Should().BeTrue();
        customer.StatusCode.Should().Be(ConnectiqApiNS.HttpStatusCode.Ok);
    }

    [Fact]
    public async Task GetCustomerById_WithInvalidId_Should_Return_NotValid_Error()
    {
        var graphqlClient = await _fixture.CreateConnectiqApiClientAsync();
        graphqlClient.Should().NotBeNull();

        var invalidId = "invalid-guid-format";

        var result = await graphqlClient!.GetCustomerById.ExecuteAsync(invalidId);
        result.Should().NotBeNull();
        result.Data.Should().BeNull("because the server should reject the request due to invalid ID format");

        result.Errors.Should().NotBeNullOrEmpty("because we expect a GraphQL error");

        var error = result.Errors!.First();

        error.Message.Should().Be("Unexpected Execution Error");
        error.Path.Should().Contain("customerById");
        error.Extensions.Should().ContainKey("message");
        error.Extensions["message"]!.ToString()
            .Should().Contain("Invalid customer ID format", "because this is the expected server-side error");

        error.Extensions.Should().ContainKey("stackTrace");
        error.Extensions["stackTrace"]!.ToString().Should().Contain("GetCustomerByIdCommandHandler");
    }

    [Fact]
    public async Task GetCustomerById_WithNonExistingId_Should_Return_NotFound_Error()
    {
        var graphqlClient = await _fixture.CreateConnectiqApiClientAsync();
        graphqlClient.Should().NotBeNull();

        var nonExistingId = Guid.NewGuid().ToString();

        var result = await graphqlClient!.GetCustomerById.ExecuteAsync(nonExistingId);

        result.Should().NotBeNull();
        result.Data.Should().BeNull("because the customer does not exist");

        result.Errors.Should().NotBeNullOrEmpty("because we expect a GraphQL error");

        var error = result.Errors!.First();

        error.Message.Should().Be("Unexpected Execution Error");
        error.Path.Should().Contain("customerById");

        error.Extensions.Should().ContainKey("message");
        error.Extensions["message"]!.ToString()
            .Should().Contain("Customer not found.");

        error.Extensions.Should().ContainKey("stackTrace");
        error.Extensions["stackTrace"]!.ToString()
            .Should().Contain("GetCustomerByIdCommandHandler", "because the handler is responsible for this exception");
    }
}

