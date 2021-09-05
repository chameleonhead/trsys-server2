using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Trsys.CopyTrading.Abstractions;
using Trsys.CopyTrading.Application;

namespace Trsys.CopyTrading.Service
{
    public class EaService : Ea.EaBase
    {
        private readonly ILogger<EaService> logger;
        private readonly IEaService service;

        public EaService(ILogger<EaService> logger, IEaService service)
        {
            this.logger = logger;
            this.service = service;
        }

        public override async Task<CommonResponse> AddSecretKey(AddSecretKeyRequest request, ServerCallContext context)
        {
            try
            {
                await service.AddSecretKeyAsync(request.Key, request.KeyType);
                return new CommonResponse()
                {
                    Result = CommonResponse.Types.Result.Success
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse()
                {
                    Result = CommonResponse.Types.Result.Failure,
                    Message = ex.Message,
                };
            }
        }

        public override async Task<CommonResponse> RemvoeSecretKey(RemvoeSecretKeyRequest request, ServerCallContext context)
        {
            try
            {
                await service.RemvoeSecretKeyAsync(request.Key, request.KeyType);
                return new CommonResponse()
                {
                    Result = CommonResponse.Types.Result.Success
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse()
                {
                    Result = CommonResponse.Types.Result.Failure,
                    Message = ex.Message,
                };
            }
        }

        public override async Task<GenerateSessionTokenResponse> GenerateSessionToken(GenerateSessionTokenRequest request, ServerCallContext context)
        {
            try
            {
                var session = await service.GenerateSessionTokenAsync(request.Key, request.KeyType);
                if (session != null)
                {
                    return new GenerateSessionTokenResponse()
                    {
                        Result = GenerateSessionTokenResponse.Types.Result.Success,
                        Token = session.Token,
                    };
                }
                else
                {
                    return new GenerateSessionTokenResponse()
                    {
                        Result = GenerateSessionTokenResponse.Types.Result.InvalidKey,
                    };
                }
            }
            catch (EaSessionAlreadyExistsException)
            {
                return new GenerateSessionTokenResponse()
                {
                    Result = GenerateSessionTokenResponse.Types.Result.KeyInUse,
                };
            }
            catch (Exception ex)
            {
                return new GenerateSessionTokenResponse()
                {
                    Result = GenerateSessionTokenResponse.Types.Result.Failure,
                    Message = ex.Message,
                };
            }
        }

        public override async Task<SessionTokenResponse> DiscardSessionToken(DiscardSessionTokenRequest request, ServerCallContext context)
        {
            try
            {
                await service.DiscardSessionTokenAsync(request.Token, request.Key, request.KeyType);
                return new SessionTokenResponse()
                {
                    Result = SessionTokenResponse.Types.Result.Success
                };
            }
            catch (EaSessionTokenNotFoundException)
            {
                return new SessionTokenResponse()
                {
                    Result = SessionTokenResponse.Types.Result.TokenNotFound,
                };
            }
            catch (EaSessionTokenInvalidException)
            {
                return new SessionTokenResponse()
                {
                    Result = SessionTokenResponse.Types.Result.TokenInvalid,
                };
            }
            catch (Exception ex)
            {
                return new SessionTokenResponse()
                {
                    Result = SessionTokenResponse.Types.Result.Failure,
                    Message = ex.Message,
                };
            }
        }

        public override async Task<SessionTokenResponse> ValidateSessionToken(ValidateSessionTokenRequest request, ServerCallContext context)
        {
            try
            {
                await service.ValidateSessionTokenAsync(request.Token, request.Key, request.KeyType);
                return new SessionTokenResponse()
                {
                    Result = SessionTokenResponse.Types.Result.Success
                };
            }
            catch (EaSessionTokenNotFoundException)
            {
                return new SessionTokenResponse()
                {
                    Result = SessionTokenResponse.Types.Result.TokenNotFound,
                };
            }
            catch (EaSessionTokenInvalidException)
            {
                return new SessionTokenResponse()
                {
                    Result = SessionTokenResponse.Types.Result.TokenInvalid,
                };
            }
            catch (Exception ex)
            {
                return new SessionTokenResponse()
                {
                    Result = SessionTokenResponse.Types.Result.Failure,
                    Message = ex.Message,
                };
            }
        }

        public override async Task<CommonResponse> PublishOrderText(PublishOrderTextRequest request, ServerCallContext context)
        {
            try
            {
                await service.PublishOrderTextAsync(request.Key, request.Text);
                return new CommonResponse()
                {
                    Result = CommonResponse.Types.Result.Success,
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse()
                {
                    Result = CommonResponse.Types.Result.Failure,
                    Message = ex.Message,
                };
            }
        }

        public override async Task<GetOrderTextResponse> GetOrderText(GetOrderTextRequest request, ServerCallContext context)
        {
            try
            {
                var orderText = await service.GetOrderTextAsync(request.Key);
                return new GetOrderTextResponse()
                {
                    Result = GetOrderTextResponse.Types.Result.Success,
                    Text = orderText.Text,
                };
            }
            catch (Exception ex)
            {
                return new GetOrderTextResponse()
                {
                    Result = GetOrderTextResponse.Types.Result.Failure,
                    Message = ex.Message,
                };
            }
        }

        public override async Task<CommonResponse> ReceiveLog(ReceiveLogRequest request, ServerCallContext context)
        {
            try
            {
                await service.ReceiveLogAsync(DateTimeOffset.UtcNow, request.Key, request.KeyType, request.Version, request.Token, request.Text);
                return new CommonResponse()
                {
                    Result = CommonResponse.Types.Result.Success,
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse()
                {
                    Result = CommonResponse.Types.Result.Failure,
                    Message = ex.Message,
                };
            }
        }
    }
}
