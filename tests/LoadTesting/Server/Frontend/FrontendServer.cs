using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Trsys.Frontend.Web;

namespace LoadTesting.Server.Frontend
{
    public class FrontendServer : IDisposable
    {
        private readonly IWebHost server;

        public FrontendServer()
        {
            var path = Assembly
                .GetAssembly(typeof(FrontendServer))
                .Location;

            server = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .UseUrls("https://localhost:5001", "http://localhost:5000")
                .UseKestrel()
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("Server/Frontend/appsettings.json");
                })
                .UseStartup<Startup>()
                .Build();
            server.StartAsync().Wait();
        }

        public HttpClient CreateClient()
        {
            return HttpClientFactory.Create("https://localhost:5001", true);
        }

        public void Dispose()
        {
            server.WaitForShutdown();
            server.Dispose();
            GC.SuppressFinalize(this);
        }

        public static FrontendServer CreateServer()
        {
            return new FrontendServer();
        }
    }
}
