using System.Threading.Tasks;

namespace Trsys.CopyTrading.Abstractions
{
    public interface IPublisherOrderStore
    {
        Task<PublisherOrderDifference> SetPublishedOrderTextAsync(string publisherKey, string text);
    }
}
