using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Trsys.Frontend.Web.Services;

namespace Trsys.Frontend.Web.Tests
{
    public static class HttpClientExtension
    {
        public static Task<HttpResponseMessage> GetAsync(this HttpClient client, string requestUri, string key, string keyType, string version = "20210609")
        {
            var message = new HttpRequestMessage(HttpMethod.Get, requestUri);
            message.Headers.Add("X-Ea-Id", key);
            message.Headers.Add("X-Ea-Type", keyType);
            message.Headers.Add("X-Ea-Version", version);
            return client.SendAsync(message);
        }
        public static Task<HttpResponseMessage> PostAsync(this HttpClient client, string requestUri, string key, string keyType, string version = "20210609", string content = "")
        {
            var message = new HttpRequestMessage(HttpMethod.Post, requestUri);
            message.Headers.Add("X-Ea-Id", key);
            message.Headers.Add("X-Ea-Type", keyType);
            message.Headers.Add("X-Ea-Version", version);
            message.Content = new StringContent(content, Encoding.UTF8, "text/plain");
            return client.SendAsync(message);
        }

        public static async Task RegisterSecretKeyAsync(this HttpClient _, string key, string keyType)
        {
            await EaService.Instance.AddValidSecretKyeAsync(key, keyType);
        }

        public static async Task<string> GenerateTokenAsync(this HttpClient client, string key, string keyType)
        {
            var response = await client.PostAsync("/api/token", key, keyType, content: key);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}