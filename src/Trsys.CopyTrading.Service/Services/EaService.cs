using Microsoft.Extensions.Logging;

namespace Trsys.CopyTrading.Service
{
    public class EaService : Ea.EaBase
    {
        private readonly ILogger<EaService> logger;

        public EaService(ILogger<EaService> logger)
        {
            this.logger = logger;
        }
    }
}
