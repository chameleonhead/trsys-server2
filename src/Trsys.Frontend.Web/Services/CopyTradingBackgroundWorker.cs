using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Trsys.CopyTrading.Abstractions;
using Trsys.CopyTrading.Application;
using Trsys.Frontend.Web.Hubs;

namespace Trsys.Frontend.Web.Services
{
    public class CopyTradingBackgroundWorker : BackgroundService
    {
        private readonly IEaService service;
        private readonly IHubContext<CopyTradingHub> hubContext;

        public CopyTradingBackgroundWorker(IEaService service, IHubContext<CopyTradingHub> hubContext)
        {
            this.service = service;
            this.hubContext = hubContext;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => service.UnsubscribeOrderTextUpdated(OnOrderUpdated));
            return Task.Run(() =>
            {
                service.SubscribeOrderTextUpdated(OnOrderUpdated);
            });
        }

        private async void OnOrderUpdated(OrderText orderText)
        {
            await hubContext.Clients.All.SendAsync("ReceiveMessage", "system", orderText.Text);
        }
    }
}