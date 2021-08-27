using System.Collections.Generic;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Application
{
    public interface IEaService
    {
        Task AddValidSecretKeyAsync(string key, string keyType);
        Task<EaSession> GenerateTokenAsync(string key, string keyType);
        Task<bool> InvalidateSessionAsync(string token, string key, string keyType);
        Task<bool> ValidateSessionAsync(string token, string key, string keyType);
        Task PublishOrderAsync(string key, string orders);
    }
}