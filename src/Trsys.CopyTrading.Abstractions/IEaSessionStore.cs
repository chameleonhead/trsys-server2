using System.Threading.Tasks;

namespace Trsys.CopyTrading.Abstractions
{
    public interface IEaSessionStore
    {
        Task<EaSession> FindByTokenAsync(string token);

        Task<EaSession> FindByKeyAsync(string key, string keyType);

        Task<EaSession> CreateSessionAsync(SecretKey secretKey);

        Task RemoveAsync(EaSession session);
    }
}
