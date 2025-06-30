using Connectiq.API.IntegrationTests.Tests;
using Connectiq.PerformanceTests.Customer;
using NBomber.Contracts.Stats;
using NBomber.CSharp;

namespace Connectiq.PerformanceTestss;

public class CustomerPerformance(ApiFixture _fixture) : IClassFixture<ApiFixture>
{
    [Theory]
    [InlineData(10, 30)]
    [InlineData(20, 30)]
    [InlineData(40, 30)]
    [InlineData(40, 60)]
    public async Task Customers_Queries_Should_Pass(int rate, int durationSeconds)
    {
        var client = await _fixture.CreateConnectiqApiClientAsync();
        var getAllCustomersScenario = QueriesPerformanceTests.GetAllCustomersScenario(client, rate, durationSeconds);
        var getCustomerByIdScenario = QueriesPerformanceTests.GetCustomerByIdScenario(client, rate, durationSeconds);

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var reportFolder = Path.Combine("nbomber-reports", "customers", "get_all", $"{timestamp}_{rate}_{durationSeconds}");

        NBomberRunner
            .RegisterScenarios(getAllCustomersScenario, getCustomerByIdScenario)
            .WithTestSuite("Connectiq Customers Performance Tests")
            .WithTestName($"Customers_rate_{rate}_duration_{durationSeconds}")
            .WithReportFileName($"customers_query_performance_{rate}rps_{durationSeconds}s")
            .WithReportFolder(reportFolder)
            .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
            .Run();
    }
}
