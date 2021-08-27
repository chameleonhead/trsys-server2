using System.Threading.Tasks;

namespace Trsys.CopyTrading.Abstractions
{
    public interface IOrderStore
    {
        Task SetTextAsync(string key, string text);
        Task<PublishedOrders> GetTextAsync(string key);
    }
}
