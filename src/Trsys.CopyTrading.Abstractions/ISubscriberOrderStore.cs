using System.Threading.Tasks;

namespace Trsys.CopyTrading.Abstractions
{
    public interface ISubscriberOrderStore
    {
        Task<OrderDifference<SubscriberOrder>> SetOrderTextAsync(string subscriberKey, ActiveOrder activeOrder);
    }
}
