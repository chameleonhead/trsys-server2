using System;
using System.Threading.Tasks;

namespace Trsys.Frontend.Web.Services
{
    public class EaSession
    {
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Token { get; set; }
    }

    public class EaService
    {
        public async Task<EaSession> GenerateTokenAsync(string key, string keyType)
        {
            return new EaSession()
            {
                Key = key,
                KeyType = keyType,
                Token = Guid.NewGuid().ToString()
            };
        }
    }
}
