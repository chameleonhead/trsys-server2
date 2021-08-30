using System.Threading.Tasks;

namespace Trsys.CopyTrading.Abstractions
{
    public interface IOrderStore
    {
        Task SetTextAsync(string publisherKey, string text);
        Task<PublishedOrders> GetTextAsync(string subscriberKey);
    }
}
