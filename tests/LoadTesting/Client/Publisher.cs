using LoadTesting.Extensions;
using NBomber.Contracts;
using Serilog;
using System.Net.Http;
using System.Threading.Tasks;

namespace LoadTesting.Client
{
    public class Publisher : TokenClientBase
    {
        private readonly OrderProvider orderProvider;
        private string sentOrder;

        public Publisher(HttpClient client, string secretKey, OrderProvider orderProvider) : base(client, secretKey, "Publisher")
        {
            this.orderProvider = orderProvider;
        }

        protected override async Task<Response> OnExecuteAsync()
        {
            var orderText = orderProvider.GetCurrentOrder();
            if (sentOrder != orderText)
            {
                try
                {
                    await Client.PublishOrderAsync(SecretKey, Token, orderText);
                }
                catch
                {
                    return Response.Fail($"Publish order failed");
                }
                Log.Logger.Information($"Publisher:{SecretKey}:OrderUpdated:{orderText}");
                sentOrder = orderText;
                return Response.Ok(payload: "Order posted" + sentOrder);
            }
            return Response.Ok(payload: "not requested");
        }
    }
}
