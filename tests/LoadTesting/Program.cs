using LoadTesting.Client;
using LoadTesting.Extensions;
using LoadTesting.Server.CopyTrading;
using LoadTesting.Server.Frontend;
using NBomber.Contracts;
using NBomber.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTesting
{

    class Program
    {
        const int COUNT_OF_CLIENTS = 50;
        const double LENGTH_OF_TEST_MINUTES = 3;

        static void Main(string[] args)
        {
            using var copyTradingServer = CopyTradingServer.CreateServer();
            using var frontendServer = FrontendServer.CreateServer();
            var publisherKey = "MT4/OANDA Corporation/899999999/2";
            var subscriberKeys = Enumerable.Range(1, COUNT_OF_CLIENTS).Select(i => $"MT4/OANDA Corporation/8{i:00000000}/2").ToList();
            WithRetry(() => RegisterKeys(frontendServer.CreateClient(), new[] { publisherKey }, "Publisher")).Wait();
            WithRetry(() => RegisterKeys(frontendServer.CreateClient(), subscriberKeys, "Subscriber")).Wait();
            var feeds = Feed.CreateConstant("secret_keys", subscriberKeys);
            var orderProvider = new OrderProvider(TimeSpan.FromMinutes(LENGTH_OF_TEST_MINUTES));
            var subscribers = subscriberKeys.Select(key => new Subscriber(frontendServer.CreateClient(), key)).ToList();
            var publisher = new Publisher(frontendServer.CreateClient(), publisherKey, orderProvider);
            orderProvider.SetStart();

            var random = new Random();
            var step1 = Step.Create("publisher", feeds, context => publisher.ExecuteAsync());
            var step2 = Step.Create("subscriber", feeds, context => subscribers[random.Next(0, COUNT_OF_CLIENTS - 1)].ExecuteAsync());

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
                    await UnregisterKeys(frontendServer.CreateClient(), new[] { publisherKey }, "Publisher");
                });


            var scenario2 = ScenarioBuilder
                .CreateScenario("sub", step2)
                .WithInit(context => Task.WhenAll(subscribers.Select(subscriber => subscriber.InitializeAsync())))
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(LoadSimulation.NewInjectPerSec(10 * COUNT_OF_CLIENTS, TimeSpan.FromMinutes(LENGTH_OF_TEST_MINUTES)))
                .WithClean(async context =>
                {
                    await Task.WhenAll(subscribers.Select(subscriber => subscriber.FinalizeAsync()));
                    await UnregisterKeys(frontendServer.CreateClient(), subscriberKeys, "Subscriber");
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

        private static async Task<bool> RegisterKeys(HttpClient httpClient, IEnumerable<string> secretKeys, string keyType)
        {
            foreach (var key in secretKeys)
            {
                await httpClient.RegisterSecretKeyAsync(key, keyType);
            }
            return true;
        }

        private static async Task UnregisterKeys(HttpClient httpClient, IEnumerable<string> secretKeys, string keyType)
        {
            foreach (var key in secretKeys)
            {
                await httpClient.UnregisterSecretKeyAsync(key, keyType);
            }
        }
    }
}

