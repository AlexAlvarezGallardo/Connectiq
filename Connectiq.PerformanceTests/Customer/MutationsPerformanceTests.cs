using ConnectiqApiNS;
using NBomber.Contracts;
using NBomber.CSharp;

namespace Connectiq.PerformanceTestss.Customer;

public class MutationsPerformanceTests
{
    static Random _random = new();

    public static Threshold[] DefaultThresholds =
    [
        Threshold.Create(
            checkScenario: stats => stats.Fail.Request.Count == 0,
            abortWhenErrorCount: 10,
            startCheckAfter: TimeSpan.FromSeconds(5)
        ),
        Threshold.Create(
            checkScenario: stats => stats.Ok.Request.RPS >= 9,
            startCheckAfter: TimeSpan.FromSeconds(5)
        ),
        Threshold.Create(
            checkScenario: stats => stats.Ok.StatusCodes.Get(System.Net.HttpStatusCode.OK.ToString()).Percent > 95,
            startCheckAfter: TimeSpan.FromSeconds(5)
        )
    ];

    public static ScenarioProps CreateCustomerScenario(IConnectiqApi client, int rate, int durationSeconds)
    {
        return Scenario.Create("create_customer", async context =>
        {
            var input = new CreateCustomerInput
            {
                Details = new CustomerDetailsInput
                {
                    Name = $"TestFirstName_{context.InvocationNumber}",
                    Address = $"TestLastName_{context.InvocationNumber}",
                    Email = $"TestEmail_{context.InvocationNumber}@example.com",
                    Phone = $"+34 {_random.Next(100, 999)} {_random.Next(100, 999)} {_random.Next(100, 999)}",
                }
            };

            try
            {
                var response = await client.CreateCustomer.ExecuteAsync(input);
                if (response.Data is not null)
                {
                    return Response.Ok(statusCode: System.Net.HttpStatusCode.OK.ToString());
                }
                return Response.Fail(statusCode: "400", message: String.Join(";", response.Errors));
            }
            catch (Exception ex)
            {
                return Response.Fail(statusCode: "500", ex.Message);
            }
        })
         .WithWarmUpDuration(TimeSpan.FromSeconds(5))
         .WithLoadSimulations(Simulation.RampingInject(rate: rate, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(durationSeconds)))
         .WithThresholds(DefaultThresholds);
    }

    public static ScenarioProps UpdateCustomerScenario(IConnectiqApi client, int rate, int durationSeconds)
    {
        return Scenario.Create("update_customer", async context =>
        {
            var getAllCustomers = await client.GetAllCustomers.ExecuteAsync(new GetAllFiltersInput
            {
                Page = 1,
                PageSize = 2000
            });

            var index = (int)(context.InvocationNumber % 2100) + 1;

            var id = getAllCustomers.Data?.AllCustomers?.Data?.Customers?[index]?.Customer!.Id ?? Guid.NewGuid().ToString();

            var input = new UpdateCustomerInput
            {
                Customer = new CustomerInput
                {
                    Id = id,
                    Details = new CustomerDetailsInput
                    {
                        Name = $"UpdatedFirstName_{context.InvocationNumber}",
                        Address = $"UpdatedLastName_{context.InvocationNumber}",
                        Email = $"TestEmail_{context.InvocationNumber}@example.com",
                        Phone = $"+34 {_random.Next(100, 999)} {_random.Next(100, 999)} {_random.Next(100, 999)}"
                    }
                }
            };

            try
            {
                var response = await client.UpdateCustomer.ExecuteAsync(input);
                if (response.Data is not null)
                {
                    return Response.Ok(statusCode: System.Net.HttpStatusCode.OK.ToString());
                }
                return Response.Fail(statusCode: "400", message: String.Join(";", response.Errors));
            }
            catch (Exception ex)
            {
                return Response.Fail(statusCode: "500", ex.Message);
            }
        })
         .WithWarmUpDuration(TimeSpan.FromSeconds(5))
         .WithLoadSimulations(Simulation.RampingInject(rate: rate, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(durationSeconds)))
         .WithThresholds(DefaultThresholds);
    }

    public static ScenarioProps SoftDeleteCustomerScenario(IConnectiqApi client, int rate, int durationSeconds)
    {
        return Scenario.Create("soft_delete_customer", async context =>
        {
            var getAllCustomers = await client.GetAllCustomers.ExecuteAsync(new GetAllFiltersInput
            {
                Page = 1,
                PageSize = 2000
            });

            var index = (int)(context.InvocationNumber % 2100) + 1;

            var id = getAllCustomers.Data?.AllCustomers?.Data?.Customers?[index]?.Customer!.Id ?? Guid.NewGuid().ToString();

            var input = new SoftDeleteCustomerInput
            {
                Id = id
            };

            try
            {
                var response = await client.SoftDeleteCustomer.ExecuteAsync(input);
                if (response.Data is not null)
                {
                    return Response.Ok(statusCode: System.Net.HttpStatusCode.OK.ToString());
                }
                return Response.Fail(statusCode: "400", message: String.Join(";", response.Errors));
            }
            catch (Exception ex)
            {
                return Response.Fail(statusCode: "500", ex.Message);
            }
        })
         .WithWarmUpDuration(TimeSpan.FromSeconds(5))
         .WithLoadSimulations(Simulation.RampingInject(rate: rate, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(durationSeconds)))
         .WithThresholds(DefaultThresholds);
    }



}
