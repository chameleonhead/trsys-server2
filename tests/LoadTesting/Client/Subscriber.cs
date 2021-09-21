using LoadTesting.Extensions;
using NBomber.Contracts;
using Serilog;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace LoadTesting.Client
{
    public class Subscriber : TokenClientBase
    {
        public Subscriber(HttpClient client, string secretKey) : base(client, secretKey, "Subscriber")
        {
        }

        public OrderResponse Order { get; private set; }

        protected override async Task<Response> OnExecuteAsync()
        {
            try
            {
                using var activity = source.StartActivity("SubscribeOrder", ActivityKind.Client);
                var order = await Client.SubscribeOrderAsync(SecretKey, Token, Order);
                if (order != Order)
                {
                    Order = order;
                    Log.Logger.Information($"Subscriber:{SecretKey}:OrderChanged:{Order.Text}");
                }
                return Response.Ok();
            }
            catch
            {
                return Response.Fail($"Subscribe order failed.");
            }
        }
    }
}
