using System;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Application
{
    public interface IEaService
    {
        Task AddSecretKeyAsync(string key, string keyType);
        Task RemvoeSecretKeyAsync(string key, string keyType);
        Task<EaSession> GenerateSessionTokenAsync(string key, string keyType);
        Task DiscardSessionTokenAsync(string token, string key, string keyType);
        Task ValidateSessionTokenAsync(string token, string key, string keyType);
        Task PublishOrderTextAsync(DateTimeOffset timestamp, string key, string text);
        Task<OrderText> GetCurrentOrderTextAsync();
        Task SubscribeOrderTextAsync(DateTimeOffset timestamp, string key, string text);
        Task ReceiveLogAsync(DateTimeOffset serverTimestamp, string key, string keyType, string version, string token, string text);
        Task ReceiveLogAsync(DateTimeOffset serverTimestamp, long eaTimestamp, string key, string keyType, string version, string token, string text);
        void SubscribeOrderTextUpdated(Action<OrderText> handler);
        void UnsubscribeOrderTextUpdated(Action<OrderText> handler);
    }
}