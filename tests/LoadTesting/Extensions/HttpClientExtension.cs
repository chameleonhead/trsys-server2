using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LoadTesting.Extensions
{
    public class OrderResponse
    {
        public string Text { get; set; }
        public string ETag { get; set; }
    }

    public static class HttpClientExtension
    {
        private readonly static ActivitySource source = new("Trsys.Server.Client");
        public static Task<HttpResponseMessage> GetAsync(this HttpClient client, string requestUri, string key, string keyType, string version = "20210609", string token = default, string ifNoneMatch = default)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, requestUri);
            message.Headers.Add("X-Ea-Id", key);
            message.Headers.Add("X-Ea-Type", keyType);
            message.Headers.Add("X-Ea-Version", version);
            if (!string.IsNullOrEmpty(token))
            {
                message.Headers.Add("X-Secret-Token", token);
            }
            if (!string.IsNullOrEmpty(ifNoneMatch))
            {
                message.Headers.Add("If-None-Match", ifNoneMatch);
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
            using var activity = source.StartActivity("RegisterSecretKey", ActivityKind.Client);
            var response = await client.PostAsync("/api/keys", key, keyType);
            response.EnsureSuccessStatusCode();
        }

        public static async Task UnregisterSecretKeyAsync(this HttpClient client, string key, string keyType)
        {
            using var activity = source.StartActivity("UnregisterSecretKey", ActivityKind.Client);
            var response = await client.PostAsync("/api/keys/delete", key, keyType);
            response.EnsureSuccessStatusCode();
        }

        public static async Task<string> GenerateTokenAsync(this HttpClient client, string key, string keyType)
        {
            using var activity = source.StartActivity("GenerateToken", ActivityKind.Client);
            var response = await client.PostAsync("/api/token", key, keyType, content: key);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task InvalidateTokenAsync(this HttpClient client, string key, string keyType, string token)
        {
            using var activity = source.StartActivity("InvalidateToken", ActivityKind.Client);
            var response = await client.PostAsync($"/api/token/{token}/release", key, keyType, content: "");
            response.EnsureSuccessStatusCode();
        }

        public static async Task PublishOrderAsync(this HttpClient client, string key, string token, string content)
        {
            using var activity = source.StartActivity("PublishOrder", ActivityKind.Client);
            var response = await client.PostAsync("/api/orders", key, "Publisher", content: content, token: token);
            response.EnsureSuccessStatusCode();
        }

        public static async Task<OrderResponse> SubscribeOrderAsync(this HttpClient client, string key, string token, OrderResponse lastResponse)
        {
            using var activity = source.StartActivity("SubscribeOrder", ActivityKind.Client);
            var response = await client.GetAsync("/api/orders", key, "Subscriber", token: token, ifNoneMatch: lastResponse?.ETag);
            if (response.StatusCode == HttpStatusCode.NotModified)
            {
                return lastResponse;
            }
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var etag = response.Headers.ETag;
            return new OrderResponse()
            {
                Text = content,
                ETag = etag.Tag,
            };
        }
    }
}