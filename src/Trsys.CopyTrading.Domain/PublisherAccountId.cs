using Akkatecture.Core;

namespace Trsys.CopyTrading.Domain
{
    public class PublisherAccountId : Identity<PublisherAccountId>
    {
        public PublisherAccountId(string value) : base(value)
        {
        }
    }
}