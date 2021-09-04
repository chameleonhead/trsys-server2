using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Trsys.CopyTrading.Service
{
    public class EaService : Ea.EaBase
    {
        private readonly ILogger<EaService> logger;

        public EaService(ILogger<EaService> logger)
        {
            this.logger = logger;
        }

        public override Task<CommonResponse> AddSecretKey(AddSecretKeyRequest request, ServerCallContext context)
        {
            return Task.FromResult(new CommonResponse()
            {
                Result = CommonResponse.Types.Result.Success
            });
        }

        public override Task<CommonResponse> RemvoeSecretKey(RemvoeSecretKeyRequest request, ServerCallContext context)
        {
            return Task.FromResult(new CommonResponse()
            {
                Result = CommonResponse.Types.Result.Success
            });
        }

        public override Task<GenerateSessionTokenResponse> GenerateSessionToken(GenerateSessionTokenRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GenerateSessionTokenResponse()
            {
                Result = GenerateSessionTokenResponse.Types.Result.Success,
                Token = "TOKEN"
            });
        }

        public override Task<SessionTokenResponse> DiscardSessionToken(DiscardSessionTokenRequest request, ServerCallContext context)
        {
            return Task.FromResult(new SessionTokenResponse()
            {
                Result = SessionTokenResponse.Types.Result.Success,
            });
        }

        public override Task<SessionTokenResponse> ValidateSessionToken(ValidateSessionTokenRequest request, ServerCallContext context)
        {
            return Task.FromResult(new SessionTokenResponse()
            {
                Result = SessionTokenResponse.Types.Result.Success,
            });
        }

        public override Task<CommonResponse> PublishOrderText(PublishOrderTextRequest request, ServerCallContext context)
        {
            return Task.FromResult(new CommonResponse()
            {
                Result = CommonResponse.Types.Result.Success
            });
        }

        public override Task<GetOrderTextResponse> GetOrderText(GetOrderTextRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetOrderTextResponse()
            {
                Result = GetOrderTextResponse.Types.Result.Success,
                Text = ""
            });
        }

        public override Task<CommonResponse> ReceiveLog(ReceiveLogRequest request, ServerCallContext context)
        {
            return Task.FromResult(new CommonResponse()
            {
                Result = CommonResponse.Types.Result.Success
            });
        }
    }
}
