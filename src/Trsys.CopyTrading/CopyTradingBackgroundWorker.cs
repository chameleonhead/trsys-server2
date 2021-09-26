using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Trsys.CopyTrading.Application;

namespace Trsys.CopyTrading
{
    public class CopyTradingBackgroundWorker : BackgroundService
    {
        private readonly GrpcEaService service;
        private readonly CopyTradingOrderTextCache cache;

        public CopyTradingBackgroundWorker(IEaService service, CopyTradingOrderTextCache cache)
        {
            this.service = service as GrpcEaService ?? throw new ArgumentNullException(nameof(service));
            this.cache = cache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var nextSyncTime = DateTime.MinValue;

            while (!stoppingToken.IsCancellationRequested)
            {
                if (nextSyncTime < DateTime.UtcNow)
                {
                    cache.UpdateOrderTextCache(await service.GetCurrentOrderTextWithoutCacheAsync());
                    nextSyncTime = nextSyncTime.AddMilliseconds(90);
                    await Task.Delay(10);
                }
            }
        }
    }
}