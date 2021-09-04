using LoadTesting.Client;
using LoadTesting.Extensions;
using LoadTesting.Server.Frontend;
using NBomber;
using NBomber.Contracts;
using NBomber.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTesting
{

    class Program
    {
        const int COUNT_OF_CLIENTS = 30;
        const double LENGTH_OF_TEST_MINUTES = 3;

        static void Main(string[] args)
        {
            using var server = FrontendServer.CreateServer();
            var secretKeys = FeedData.FromJson<string>(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Setup/SecretKeys.json"));
            WithRetry(() => RegisterKeys(server.CreateClient(), secretKeys)).Wait();
            var feeds = Feed.CreateConstant("secret_keys", secretKeys);
            var orderProvider = new OrderProvider(TimeSpan.FromMinutes(LENGTH_OF_TEST_MINUTES));
            var subscribers = Enumerable.Range(1, COUNT_OF_CLIENTS).Select(i => new Subscriber(server.CreateClient(), secretKeys.Skip(i).First())).ToList();
            var publisher = new Publisher(server.CreateClient(), secretKeys.First(), orderProvider);
            orderProvider.SetStart();

            var step1 = Step.Create("publisher", feeds, context => publisher.ExecuteAsync());
            var step2 = Step.Create("subscriber", feeds, context => subscribers[context.InvocationCount % COUNT_OF_CLIENTS].ExecuteAsync());

            var scenario1 = ScenarioBuilder
                .CreateScenario("pub", step1)
                .WithInit(async context =>
                {
                    await publisher.InitializeAsync();
                    await publisher.ExecuteAsync();
                })
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(LoadSimulation.NewInjectPerSec(1, TimeSpan.FromMinutes(LENGTH_OF_TEST_MINUTES)))
                .WithClean(async context =>
                {
                    await Task.WhenAll(publisher.FinalizeAsync());
                    await UnregisterKeys(server.CreateClient(), secretKeys.Take(1));
                });


            var scenario2 = ScenarioBuilder
                .CreateScenario("sub", step2)
                .WithInit(context => Task.WhenAll(subscribers.Select(subscriber => subscriber.InitializeAsync())))
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(LoadSimulation.NewInjectPerSec(10 * COUNT_OF_CLIENTS, TimeSpan.FromMinutes(LENGTH_OF_TEST_MINUTES)))
                .WithClean(async context =>
                {
                    await Task.WhenAll(subscribers.Select(subscriber => subscriber.FinalizeAsync()));
                    await UnregisterKeys(server.CreateClient(), secretKeys.Skip(1));
                });

            NBomberRunner
                .RegisterScenarios(scenario1, scenario2)
                .Run();

        }

        private static async Task<T> WithRetry<T>(Func<Task<T>> func)
        {
            int retryCount = 0;
            Exception lastException = null;
            while (retryCount < 10)
            {
                try
                {
                    return await Task.Run(async () => await func());
                }
                catch (Exception e)
                {
                    lastException = e;
                    Thread.Sleep(1000);
                    retryCount++;
                }
            }
            throw new Exception("Failed to execute.", lastException);
        }

        private static async Task<bool> RegisterKeys(HttpClient httpClient, IEnumerable<string> secretKeys)
        {
            foreach (var key in secretKeys)
            {
                await httpClient.RegisterSecretKeyAsync(key, "Publisher");
                await httpClient.RegisterSecretKeyAsync(key, "Subscriber");
            }
            return true;
        }

        private static async Task UnregisterKeys(HttpClient httpClient, IEnumerable<string> secretKeys)
        {
            foreach (var key in secretKeys)
            {
                await httpClient.UnregisterSecretKeyAsync(key, "Publisher");
                await httpClient.UnregisterSecretKeyAsync(key, "Subscriber");
            }
        }
    }
}

