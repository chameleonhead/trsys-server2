using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Trsys.CopyTrading.Service;

namespace LoadTesting.Server.CopyTrading
{
    public class CopyTradingServer : IDisposable
    {
        private readonly TestServer testServer;

        public CopyTradingServer()
        {
            var path = Assembly.GetAssembly(typeof(CopyTradingServer))
              .Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("Server/CopyTrading/appsettings.json");
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
        public static CopyTradingServer CreateServer()
        {
            return new CopyTradingServer();
        }
    }
}
