using Trsys.CopyTrading.Events;
using Trsys.CopyTrading.EaLogs;
using Trsys.Events.Abstractions;

namespace Trsys.CopyTrading.Application
{
    public class LogEventMapper
    {
        public static IEvent Map(ILogInfo logInfo)
        {
            switch (logInfo)
            {
                case LocalOrderOpenedLog log:
                    return new EaLocalOrderOpenedEvent(log.Timestamp, log.Key, log.KeyType, log.Version, log.Token, log.ServerTicketNo, log.LocalTicketNo, log.Symbol, log.OrderType.ToString());
                case LocalOrderClosedLog log:
                    return new EaLocalOrderClosedEvent(log.Timestamp, log.Key, log.KeyType, log.Version, log.Token, log.ServerTicketNo, log.LocalTicketNo, log.Symbol, log.OrderType.ToString());
            }
            return default;
        }
    }
}