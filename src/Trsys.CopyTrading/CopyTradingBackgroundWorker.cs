using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;
using Trsys.CopyTrading.Service;

namespace Trsys.CopyTrading
{
    public class CopyTradingBackgroundWorker : BackgroundService
    {
        private readonly ILogger<CopyTradingBackgroundWorker> logger;
        private readonly CopyTradingOrderTextCache cache;
        private readonly EaServicePool pool;

        public CopyTradingBackgroundWorker(ILogger<CopyTradingBackgroundWorker> logger, CopyTradingOrderTextCache cache, EaServicePool pool)
        {
            this.logger = logger;
            this.cache = cache;
            this.pool = pool;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var nextSyncTime = DateTime.MinValue;

            while (!stoppingToken.IsCancellationRequested)
            {
                if (nextSyncTime < DateTime.UtcNow)
                {
                    try
                    {
                        using (var activity = CopyTradingActivitySource.Source.StartActivity("CopyTradingBackgroundWorker.UpdateOrderTextCache"))
                        {
                            var service = pool.ServiceForBackgroundWorker;
                            var response = await service.GetCurrentOrderTextAsync(new GetCurrentOrderTextRequest());
                            if (response.Result == GetCurrentOrderTextResponse.Types.Result.Success)
                            {
                                if (cache.GetOrderTextCache()?.Text != response.Text)
                                {
                                    cache.UpdateOrderTextCache(OrderText.Parse(response.Text));
                                    Activity.Current?.AddEvent(new ActivityEvent("CacheUpdated"));
                                }
                            }

                            nextSyncTime = nextSyncTime.AddMilliseconds(100);
                            if (nextSyncTime < DateTime.UtcNow)
                            {
                                nextSyncTime = DateTime.UtcNow.AddMilliseconds(100);
                            }
                            await Task.Delay(10);
                        }
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex, "Unknown error");
                    }
                }
            }
        }
    }
}