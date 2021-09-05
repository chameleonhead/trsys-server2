using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Trsys.CopyTrading.Service;

namespace LoadTesting.Server.CopyTrading
{
    public class CopyTradingServer : IDisposable
    {
        private readonly IWebHost server;

        public CopyTradingServer()
        {
            var path = Assembly
                .GetAssembly(typeof(CopyTradingServer))
                .Location;

            server = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .UseUrls("https://localhost:5003", "http://localhost:5002")
                .UseKestrel()
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("Server/CopyTrading/appsettings.json");
                })
                .UseStartup<Startup>()
                .Build();
            server.StartAsync().Wait();
        }

        public HttpClient CreateClient()
        {
            return HttpClientFactory.Create("https://localhost:5003", true);
        }

        public void Dispose()
        {
            server.StopAsync();
            server.WaitForShutdown();
            server.Dispose();
            GC.SuppressFinalize(this);
        }
        public static CopyTradingServer CreateServer()
        {
            return new CopyTradingServer();
        }
    }
}
