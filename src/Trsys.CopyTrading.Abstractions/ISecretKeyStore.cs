using System.Threading.Tasks;

namespace Trsys.CopyTrading.Abstractions
{
    public interface ISecretKeyStore
    {
        Task<SecretKey> FindAsync(string key, string keyType);
        Task AddAsync(string key, string keyType);
        Task RemoveAsync(string key, string keyType);
    }
}
