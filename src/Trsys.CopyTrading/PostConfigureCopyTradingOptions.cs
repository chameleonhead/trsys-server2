using Grpc.Net.Client;
using Microsoft.Extensions.Options;

namespace Trsys.CopyTrading
{
    public class PostConfigureCopyTradingOptions : IPostConfigureOptions<CopyTradingOptions>
    {
        public void PostConfigure(string name, CopyTradingOptions options)
        {
            if (!string.IsNullOrEmpty(options.ServiceEndopoint))
            {
                options.Channel = GrpcChannel.ForAddress(options.ServiceEndopoint);
            }
        }
    }
}
