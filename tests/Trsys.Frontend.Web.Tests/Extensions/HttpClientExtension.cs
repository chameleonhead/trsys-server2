using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Trsys.Frontend.Web.Tests
{
    public static class HttpClientExtension
    {
        public static Task<HttpResponseMessage> GetAsync(this HttpClient client, string requestUri, string key, string keyType, string version = "20210609", string token = default)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, requestUri);
            message.Headers.Add("X-Ea-Id", key);
            message.Headers.Add("X-Ea-Type", keyType);
            message.Headers.Add("X-Ea-Version", version);
            if (!string.IsNullOrEmpty(token))
            {
                message.Headers.Add("X-Secret-Token", token);
            }
            return client.SendAsync(message);
        }
        public static Task<HttpResponseMessage> PostAsync(this HttpClient client, string requestUri, string key, string keyType, string version = "20210609", string content = "", string token = default)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, requestUri);
            message.Headers.Add("X-Ea-Id", key);
            message.Headers.Add("X-Ea-Type", keyType);
            message.Headers.Add("X-Ea-Version", version);
            if (!string.IsNullOrEmpty(token))
            {
                message.Headers.Add("X-Secret-Token", token);
            }
            message.Content = new StringContent(content, Encoding.UTF8, "text/plain");
            return client.SendAsync(message);
        }

        public static async Task RegisterSecretKeyAsync(this HttpClient client, string key, string keyType)
        {
            var response = await client.PostAsync("/api/keys", key, keyType);
            response.EnsureSuccessStatusCode();
        }

        public static async Task<string> GenerateTokenAsync(this HttpClient client, string key, string keyType)
        {
            var response = await client.PostAsync("/api/token", key, keyType, content: key);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task PublishOrderAsync(this HttpClient client, string key, string token, string content)
        {
            var response = await client.PostAsync("/api/orders", key, "Publisher", content: content, token: token);
            response.EnsureSuccessStatusCode();
        }
    }
}