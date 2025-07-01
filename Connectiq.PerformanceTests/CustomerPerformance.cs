using Connectiq.API.IntegrationTests.Tests;
using Connectiq.PerformanceTests.Customer;
using Connectiq.PerformanceTestss.Customer;
using ConnectiqApiNS;
using NBomber.Contracts.Stats;
using NBomber.CSharp;

namespace Connectiq.PerformanceTests;

public class CustomerPerformance(ApiFixture _fixture) : IClassFixture<ApiFixture>
{
    [Theory(Skip = "Should Run Manually")]
    [InlineData(15, 30)]
    [InlineData(20, 30)]
    [InlineData(40, 30)]
    public async Task Customers_Queries_Should_Pass(int rate, int durationSeconds)
    {
        var client = await _fixture.CreateConnectiqApiClientAsync();
        var getAllCustomersScenario = QueriesPerformanceTests.GetAllCustomersScenario(client, rate, durationSeconds);
        var getAllCustomers = await client.GetAllCustomers.ExecuteAsync(new GetAllFiltersInput { Page = 1, PageSize = 2000 });
        var getCustomerByIdScenario = QueriesPerformanceTests.GetCustomerByIdScenario(client, rate, durationSeconds, getAllCustomers);

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var reportFolder = Path.Combine("nbomber-reports", "customers", "queries", $"{timestamp}_{rate}_{durationSeconds}");

        NBomberRunner
            .RegisterScenarios(getAllCustomersScenario, getCustomerByIdScenario)
            .WithTestSuite("Connectiq Customers Queries Performance Tests")
            .WithTestName($"Customers_rate_{rate}_duration_{durationSeconds}")
            .WithReportFileName($"customers_query_performance_{rate}rps_{durationSeconds}s")
            .WithReportFolder(reportFolder)
            .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
            .Run();
    }

    [Theory(Skip = "Should Run Manually")]
    [InlineData(20, 30)]
    [InlineData(25, 30)]
    public async Task Customers_Mutations_Should_Pass(int rate, int durationSeconds)
    {
        var client = await _fixture.CreateConnectiqApiClientAsync();
        var createCustomerScenario = MutationsPerformanceTests.CreateCustomerScenario(client, rate, durationSeconds);
        var updateCustomerScenario = MutationsPerformanceTests.UpdateCustomerScenario(client, rate, durationSeconds);
        var softDeleteCustomerScenario = MutationsPerformanceTests.SoftDeleteCustomerScenario(client, rate, durationSeconds);

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var reportFolder = Path.Combine("nbomber-reports", "customers", "mutations", $"{timestamp}_{rate}_{durationSeconds}");

        NBomberRunner
            .RegisterScenarios(createCustomerScenario, updateCustomerScenario, softDeleteCustomerScenario)
            .WithTestSuite("Connectiq Customers Mutations Performance Tests")
            .WithTestName($"Customers_rate_{rate}_duration_{durationSeconds}")
            .WithReportFileName($"customers_mutation_performance_{rate}rps_{durationSeconds}s")
            .WithReportFolder(reportFolder)
            .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
            .Run();
    }
}
