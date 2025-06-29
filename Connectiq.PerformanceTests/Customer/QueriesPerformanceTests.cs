using Connectiq.API.IntegrationTests.Tests;
using ConnectiqApiNS;
using NBomber.Contracts;
using NBomber.Contracts.Stats;
using NBomber.CSharp;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace Connectiq.PerformanceTests.Customer;

public class QueriesPerformanceTests(ApiFixture _fixture): IClassFixture<ApiFixture>
{
    public static Threshold[] Thres = new[]
        {
            Threshold.Create(
                checkScenario: stats => stats.Fail.Request.Count == 0,
                abortWhenErrorCount: 10,
                startCheckAfter: TimeSpan.FromSeconds(5)
            ),
            Threshold.Create(
                checkScenario: stats => stats.Ok.Request.RPS >= 10,
                startCheckAfter: TimeSpan.FromSeconds(5)
            ),
            Threshold.Create(
                checkScenario: stats => stats.Ok.StatusCodes.Get(HttpStatusCode.OK.ToString()).Percent > 90,
                startCheckAfter: TimeSpan.FromSeconds(5)
            )
        };

    private ScenarioProps CreateScenario(IConnectiqApi client, int rate, int durationSeconds)
    {
        var thresholds = new[]
        {
            Threshold.Create(
                checkScenario: stats => stats.Fail.Request.Count == 0,
                abortWhenErrorCount: 10, 
                startCheckAfter: TimeSpan.FromSeconds(5) 
            ),
            Threshold.Create(
                checkScenario: stats => stats.Ok.Request.RPS >= 10,
                startCheckAfter: TimeSpan.FromSeconds(5)
            ),
            Threshold.Create(
                checkScenario: stats => stats.Ok.StatusCodes.Get(HttpStatusCode.OK.ToString()).Percent > 90,
                startCheckAfter: TimeSpan.FromSeconds(5)
            )
        };

        return Scenario.Create("get_all_customers", async context =>
        {
            var page = (int)(context.InvocationNumber % 3) + 1;
            var pageSize = 200;

            var input = new GetAllFiltersInput
            {
                Page = page,
                PageSize = pageSize
            };

            try
            {
                var response = await client.GetAllCustomers.ExecuteAsync(input);

                if (response.Data is not null)
                {
                    var castResponse = (IGetAllCustomers_AllCustomers_QueryResponseOfGetCustomersResponse)response.Data!.AllCustomers;
                    return Response.Ok(statusCode: HttpStatusCode.OK.ToString());
                }

                return Response.Fail(statusCode: "500", message: String.Join(";", response.Errors));

            }
            catch (Exception ex)
            {
                return Response.Fail(statusCode: "500", ex.Message);
            }
        })
        .WithWarmUpDuration(TimeSpan.FromSeconds(5))
        .WithLoadSimulations(Simulation.Inject(rate: rate, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(durationSeconds)))
        .WithThresholds(thresholds);
    }

    [Theory]
    [InlineData(10, 30)]
    [InlineData(20, 30)]
    [InlineData(40, 30)]
    [InlineData(40, 60)]
    public async Task LoadTest_GetAllCustomersAsync(int rate, int durationSeconds)
    {
        var client = await _fixture.CreateConnectiqApiClientAsync();
        var scenario = CreateScenario(client, rate, durationSeconds);

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var reportFolder = Path.Combine("nbomber-reports", "customers", "get_all", $"{timestamp}_{rate}_{durationSeconds}");

        NBomberRunner
            .RegisterScenarios(scenario)
            .WithTestSuite("Connectiq Customers Performance Tests")
            .WithTestName($"Customers_rate_{rate}_duration_{durationSeconds}")
            .WithReportFileName($"customers_query_performance_{rate}rps_{durationSeconds}s")
            .WithReportFolder(reportFolder)
            .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
            .Run();

    }
}
