using System.Collections.Generic;
using System.Threading.Tasks;

namespace Trsys.CopyTrading.Abstractions
{
    public interface IValidSecretKeyStore
    {
        Task<SecretKey> FindAsync(string key);
        Task AddPublisherAsync(string key);
        Task AddSubscriberAsync(string key, string following = null);
        Task RemoveAsync(string key);
        Task<IEnumerable<SecretKey>> SearchFollowersAsync(string key);
    }
}
