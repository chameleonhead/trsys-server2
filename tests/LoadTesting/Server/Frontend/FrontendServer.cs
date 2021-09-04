using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
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
        private readonly TestServer testServer;

        public FrontendServer()
        {
            var path = Assembly.GetAssembly(typeof(FrontendServer))
              .Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("Server/Frontend/appsettings.json");
                }).UseStartup<Startup>();

            testServer = new TestServer(hostBuilder);
        }

        public HttpClient CreateClient()
        {
            return testServer.CreateClient();
        }

        public void Dispose()
        {
            testServer.Dispose();
            GC.SuppressFinalize(this);
        }
        public static FrontendServer CreateServer()
        {
            return new FrontendServer();
        }
    }
}
