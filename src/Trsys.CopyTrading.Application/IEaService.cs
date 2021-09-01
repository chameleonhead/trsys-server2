using System;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Application
{
    public interface IEaService
    {
        Task AddSecretKeyAsync(string key, string keyType);
        Task<EaSession> GenerateTokenAsync(string key, string keyType);
        Task<bool> InvalidateSessionAsync(string token, string key, string keyType);
        Task<bool> ValidateSessionAsync(string token, string key, string keyType);
        Task PublishOrderTextAsync(string key, string text);
        Task<OrderText> GetOrderTextAsync(string key);
        Task ReceiveLogAsync(string key, string keyType, string version, string token, string text);
        Task ReceiveLogAsync(DateTimeOffset serverTimestamp, long eaTimestamp, string key, string keyType, string version, string token, string text);
    }
}