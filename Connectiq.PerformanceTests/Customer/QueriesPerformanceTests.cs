using ConnectiqApiNS;
using NBomber.Contracts;
using NBomber.CSharp;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace Connectiq.PerformanceTests.Customer;

public class QueriesPerformanceTests
{
    static readonly Random _random = new ();

    public static Threshold[] DefaultThresholds =
    [
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
    ];

    public static ScenarioProps GetAllCustomersScenario(IConnectiqApi client, int rate, int durationSeconds)
    {
        return Scenario.Create("get_all_customers", async context =>
        {
            var page = (int)(context.InvocationNumber % 3) + 1;
            var pageSize = _random.Next(200, 2001);

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
        .WithThresholds(DefaultThresholds);
    }

    public static ScenarioProps GetCustomerByIdScenario(IConnectiqApi client, int rate, int durationSeconds)
    {
        return Scenario.Create("get_customer_by_id", async context =>
        {
            var getAllCustomers = await client.GetAllCustomers.ExecuteAsync(new GetAllFiltersInput
            {
                Page = 1,
                PageSize = 2000
            });

            var index = (int)(context.InvocationNumber % 2100) + 1;

            var id = getAllCustomers.Data?.AllCustomers?.Data?.Customers?[index]?.Customer!.Id ?? Guid.NewGuid().ToString();

            try
            {
                var response = await client.GetCustomerById.ExecuteAsync(id);
                if (response.Data is not null)
                {
                    return Response.Ok(statusCode: HttpStatusCode.OK.ToString());
                }
                return Response.Fail(statusCode: "404", message: String.Join(";", response.Errors));
            }
            catch (Exception ex)
            {
                return Response.Fail(statusCode: "500", ex.Message);
            }
        })
        .WithWarmUpDuration(TimeSpan.FromSeconds(5))
        .WithLoadSimulations(Simulation.Inject(rate: rate, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(durationSeconds)))
        .WithThresholds(DefaultThresholds);
    }
}