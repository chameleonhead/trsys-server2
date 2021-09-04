using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Trsys.CopyTrading.Abstractions;
using Trsys.CopyTrading.Application;
using Trsys.CopyTrading.Service;

namespace Trsys.CopyTrading
{
    class GrpcEaService : IEaService
    {
        private readonly GrpcChannel channel;

        public GrpcEaService(GrpcChannel channel)
        {
            this.channel = channel;
        }
        public async Task AddSecretKeyAsync(string key, string keyType)
        {
            var service = new Ea.EaClient(channel);
            var response = await service.AddSecretKeyAsync(new AddSecretKeyRequest()
            {
                Key = key,
                KeyType = keyType,
            });
            switch (response.Result)
            {
                case CommonResponse.Types.Result.Success:
                    break;
                default:
                    throw new Exception(response.Message);
            }
        }

        public async Task RemvoeSecretKeyAsync(string key, string keyType)
        {
            var service = new Ea.EaClient(channel);
            var response = await service.RemvoeSecretKeyAsync(new RemvoeSecretKeyRequest()
            {
                Key = key,
                KeyType = keyType,
            });
            switch (response.Result)
            {
                case CommonResponse.Types.Result.Success:
                    break;
                default:
                    throw new Exception(response.Message);
            }
        }

        public async Task<EaSession> GenerateSessionTokenAsync(string key, string keyType)
        {
            var service = new Ea.EaClient(channel);
            var response = await service.GenerateSessionTokenAsync(new GenerateSessionTokenRequest()
            {
                Key = key,
                KeyType = keyType,
            });
            switch (response.Result)
            {
                case GenerateSessionTokenResponse.Types.Result.Success:
                    return new EaSession()
                    {
                        Key = key,
                        KeyType = keyType,
                        Token = response.Token
                    };
                case GenerateSessionTokenResponse.Types.Result.KeyInUse:
                    throw new EaSessionAlreadyExistsException();
                case GenerateSessionTokenResponse.Types.Result.InvalidKey:
                    return null;
                default:
                    throw new Exception(response.Message);
            }
        }

        public async Task DiscardSessionTokenAsync(string token, string key, string keyType)
        {
            var service = new Ea.EaClient(channel);
            var response = await service.DiscardSessionTokenAsync(new DiscardSessionTokenRequest()
            {
                Key = key,
                KeyType = keyType,
            });
            switch (response.Result)
            {
                case SessionTokenResponse.Types.Result.Success:
                    break;
                case SessionTokenResponse.Types.Result.TokenNotFound:
                    throw new EaSessionTokenNotFoundException();
                case SessionTokenResponse.Types.Result.TokenInvalid:
                    throw new EaSessionTokenInvalidException();
                default:
                    throw new Exception(response.Message);
            }
        }

        public async Task ValidateSessionTokenAsync(string token, string key, string keyType)
        {
            var service = new Ea.EaClient(channel);
            var response = await service.ValidateSessionTokenAsync(new ValidateSessionTokenRequest()
            {
                Key = key,
                KeyType = keyType,
            });
            switch (response.Result)
            {
                case SessionTokenResponse.Types.Result.Success:
                    break;
                case SessionTokenResponse.Types.Result.TokenNotFound:
                    throw new EaSessionTokenNotFoundException();
                case SessionTokenResponse.Types.Result.TokenInvalid:
                    throw new EaSessionTokenInvalidException();
                default:
                    throw new Exception(response.Message);
            }
        }

        public async Task PublishOrderTextAsync(string key, string text)
        {
            var service = new Ea.EaClient(channel);
            var response = await service.PublishOrderTextAsync(new PublishOrderTextRequest()
            {
                Key = key,
                Text = text,
            });
            switch (response.Result)
            {
                case CommonResponse.Types.Result.Success:
                    break;
                default:
                    throw new Exception(response.Message);
            }
        }

        public async Task<OrderText> GetOrderTextAsync(string key)
        {
            var service = new Ea.EaClient(channel);
            var response = await service.GetOrderTextAsync(new GetOrderTextRequest()
            {
                Key = key,
            });
            switch (response.Result)
            {
                case GetOrderTextResponse.Types.Result.Success:
                    return OrderText.Parse(response.Text);
                default:
                    throw new Exception(response.Message);
            }
        }

        public async Task ReceiveLogAsync(DateTimeOffset serverTimestamp, string key, string keyType, string version, string token, string text)
        {
            var service = new Ea.EaClient(channel);
            var response = await service.ReceiveLogAsync(new ReceiveLogRequest()
            {
                Key = key,
                KeyType = keyType,
                Version = version,
                Token = token,
                Text = text,
            });
            switch (response.Result)
            {
                case CommonResponse.Types.Result.Success:
                    break;
                default:
                    throw new Exception(response.Message);
            }
        }

        public async Task ReceiveLogAsync(DateTimeOffset serverTimestamp, long eaTimestamp, string key, string keyType, string version, string token, string text)
        {
            var service = new Ea.EaClient(channel);
            var response = await service.ReceiveLogAsync(new ReceiveLogRequest()
            {
                Key = key,
                KeyType = keyType,
                Version = version,
                Token = token,
                Text = text,
            });
            switch (response.Result)
            {
                case CommonResponse.Types.Result.Success:
                    break;
                default:
                    throw new Exception(response.Message);
            }
        }
    }
}
