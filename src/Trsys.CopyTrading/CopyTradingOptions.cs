using Grpc.Net.Client;

namespace Trsys.CopyTrading
{
    public class CopyTradingOptions
    {
        public string ServiceEndopoint { get; set; }
        internal GrpcChannel Channel { get; set; }
    }
}
