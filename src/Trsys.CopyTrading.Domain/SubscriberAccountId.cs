using Akkatecture.Core;

namespace Trsys.CopyTrading.Domain
{
    public class SubscriberAccountId : Identity<SubscriberAccountId>
    {
        public SubscriberAccountId(string value) : base(value)
        {
        }
    }
}
