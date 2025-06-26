using Aspire.Hosting;
using ConnectiqApiNS;
namespace Connectiq.API.IntegrationTests.Tests;

public class ApiFixture : IAsyncLifetime
{
    public DistributedApplication? _app;
    ResourceNotificationService? _notificationService;
    HttpClient? _testingClient;
    IServiceProvider? _serviceProvider;
    IConnectiqApi? _graphQLClient;

    public async Task InitializeAsync()
    {
        var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Connectiq_AppHost>();

        _app = await builder.BuildAsync();

        _notificationService = _app.Services.GetRequiredService<ResourceNotificationService>();
        await _app.StartAsync();
        await _notificationService.WaitForResourceAsync("connectiq-api", KnownResourceStates.Running)
                                 .WaitAsync(TimeSpan.FromSeconds(30));

        var services = new ServiceCollection();

        var graphqlUri = new Uri(_app.GetEndpoint("connectiq-api"), "/graphql");
        services.AddConnectiqApi().ConfigureHttpClient(c => c.BaseAddress = graphqlUri);

        _serviceProvider = services.BuildServiceProvider();

        _graphQLClient = _serviceProvider.GetRequiredService<IConnectiqApi>();
    }

    public async Task<HttpClient> CreateHttpClientAsync(string applicationName) 
    { 
        if(_testingClient is not null)
            return _testingClient;

        _testingClient = _app!.CreateHttpClient(applicationName);

        await _notificationService!.WaitForResourceAsync(applicationName, KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        return _testingClient;
    }

    public async Task<IConnectiqApi> CreateConnectiqApiClientAsync() 
    {
        if (_graphQLClient is not null)
            return _graphQLClient;

        await _notificationService!.WaitForResourceAsync("connectiq-api", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(60));

        _graphQLClient = _app!.Services.GetRequiredService<IConnectiqApi>();

        _graphQLClient!.GetAllCustomers.WithRequestUri(_app.GetEndpoint("connectiq-api"));
        _graphQLClient!.GetCustomerById.WithRequestUri(_app.GetEndpoint("connectiq-api"));

        _graphQLClient!.CreateCustomer.WithRequestUri(_app.GetEndpoint("connectiq-api"));
        _graphQLClient!.UpdateCustomer.WithRequestUri(_app.GetEndpoint("connectiq-api"));

        return _graphQLClient;
    }

    public async Task DisposeAsync()
    {
        if (_app is not null)
            await _app.DisposeAsync().ConfigureAwait(false);
    }
}
