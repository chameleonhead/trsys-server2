﻿using NBomber.Contracts;
using Serilog;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LoadTesting.Client
{
    public class Subscriber : TokenClientBase
    {
        private string orderText;
        private string orderHash;

        public Subscriber(HttpClient client, string secretKey) : base(client, secretKey)
        {
        }

        protected override async Task<Response> OnExecuteAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/orders");
            if (!string.IsNullOrEmpty(orderHash))
            {
                request.Headers.Add("If-None-Match", orderHash);
            }
            var res = await Client.SendAsync(request);
            if (res.StatusCode == HttpStatusCode.NotModified)
            {
                return Response.Ok();
            }
            if (!res.IsSuccessStatusCode)
            {
                return Response.Fail($"Order response is not valid. Status code = {res.StatusCode}");
            }

            orderHash = res.Headers.ETag.Tag;
            orderText = await res.Content.ReadAsStringAsync();
            Log.Logger.Information($"Subscriber:{SecretKey}:OrderChanged:{orderText}");
            await Client.PostAsync("/api/logs", new StringContent($"OrderChanged:{orderText}", Encoding.UTF8, "text/plain"));
            return Response.Ok();
        }
    }
}
