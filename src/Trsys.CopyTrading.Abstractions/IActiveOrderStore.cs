using System.Threading.Tasks;

namespace Trsys.CopyTrading.Abstractions
{
    public interface IActiveOrderStore
    {
        Task<ActiveOrder> GetActiveOrderAsync();
        Task<ActiveOrderSetResult> ApplyChangesAsync(PublisherOrderDifference diff);
    }
}
