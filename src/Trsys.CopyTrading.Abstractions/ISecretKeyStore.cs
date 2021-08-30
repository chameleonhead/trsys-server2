using System.Threading.Tasks;

namespace Trsys.CopyTrading.Abstractions
{
    public interface ISecretKeyStore
    {
        Task<SecretKey> FindAsync(string key, string keyType);
        Task<SecretKey> AddAsync(string key, string keyType);
        Task<SecretKey> RemoveAsync(string key, string keyType);
    }
}
