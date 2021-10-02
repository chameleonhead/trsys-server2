using Akkatecture.Core;

namespace Trsys.CopyTrading.Domain
{
    public class CopyTradeId : Identity<CopyTradeId>
    {
        public CopyTradeId(string value) : base(value)
        {
        }
    }
}