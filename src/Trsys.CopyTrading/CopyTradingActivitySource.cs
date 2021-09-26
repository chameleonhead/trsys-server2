using System.Diagnostics;

namespace Trsys.CopyTrading
{
    static class CopyTradingActivitySource
    {
        public static ActivitySource Source { get; } = new ActivitySource("Trsys.CopyTrading");
    }
}
