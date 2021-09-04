using LoadTesting.Extensions;
using NBomber.Contracts;
using Serilog;
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
                var order = await Client.SubscribeOrderAsync(SecretKey, "Subscriber", Order);
                if (order != Order)
                {
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
