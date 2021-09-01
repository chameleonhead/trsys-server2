using System.Threading.Tasks;

namespace Trsys.CopyTrading.Abstractions
{
    public interface IPublisherOrderStore
    {
        Task<OrderDifference<PublisherOrder>> SetOrderTextAsync(string publisherKey, string text);
    }
}
