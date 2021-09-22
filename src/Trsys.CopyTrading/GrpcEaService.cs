using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;
using Trsys.CopyTrading.Application;
using Trsys.CopyTrading.Service;

namespace Trsys.CopyTrading
{
    class GrpcEaService : IEaService
    {
        private readonly static ActivitySource source = new ActivitySource("Trsys.CopyTrading.GrpcClient");

        private readonly GrpcChannel channel;
        private readonly Ea.EaClient service;

        public GrpcEaService(GrpcChannel channel)
        {
            this.channel = channel;
            this.service = new Ea.EaClient(channel);
        }
        public async Task AddSecretKeyAsync(string key, string keyType)
        {
            using var activity = source.StartActivity("AddSecretKeyAsync");
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
            using var activity = source.StartActivity("RemvoeSecretKeyAsync");
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
            using var activity = source.StartActivity("GenerateSessionTokenAsync");
            var response = await service.GenerateSessionTokenAsync(new GenerateSessionTokenRequest()
            {
                Key = key,
                KeyType = keyType,
            });
            switch (response.Result)
            {
                case GenerateSessionTokenResponse.Types.Result.Success:
                    return new EaSession(key, keyType, response.Token);
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
            using var activity = source.StartActivity("DiscardSessionTokenAsync");
            var response = await service.DiscardSessionTokenAsync(new DiscardSessionTokenRequest()
            {
                Token = token,
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
            using var activity = source.StartActivity("ValidateSessionTokenAsync");
            var service = new Ea.EaClient(channel);
            var response = await service.ValidateSessionTokenAsync(new ValidateSessionTokenRequest()
            {
                Token = token,
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
            using var activity = source.StartActivity("PublishOrderTextAsync");
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

        public async Task<OrderText> GetCurrentOrderTextAsync()
        {
            using var activity = source.StartActivity("GetCurrentOrderTextAsync");
            var service = new Ea.EaClient(channel);
            var response = await service.GetCurrentOrderTextAsync(new GetCurrentOrderTextRequest());
            switch (response.Result)
            {
                case GetCurrentOrderTextResponse.Types.Result.Success:
                    return OrderText.Parse(response.Text);
                default:
                    throw new Exception(response.Message);
            }
        }

        public async Task SubscribeOrderTextAsync(string key, string text)
        {
            using var activity = source.StartActivity("SubscribeOrderTextAsync");
            var response = await service.SubscribeOrderTextAsync(new SubscribeOrderTextRequest()
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

        public async Task ReceiveLogAsync(DateTimeOffset serverTimestamp, string key, string keyType, string version, string token, string text)
        {
            using var activity = source.StartActivity("ReceiveLogAsync");
            var response = await service.ReceiveLogAsync(new ReceiveLogRequest()
            {
                ServerTimestamp = Timestamp.FromDateTimeOffset(serverTimestamp),
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
            using var activity = source.StartActivity("ReceiveLogAsync");
            var response = await service.ReceiveLogAsync(new ReceiveLogRequest()
            {
                ServerTimestamp = Timestamp.FromDateTimeOffset(serverTimestamp),
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

        public async void SubscribeSubscriberOrderUpdate(Action<string, OrderText> handler)
        {
            using var activity = source.StartActivity("SubscribeSubscriberOrderUpdate");
            using var stream = service.GetCurrentOrderTextStream(new GetCurrentOrderTextRequest());
            var reader = stream.ResponseStream;
            while (await reader.MoveNext(CancellationToken.None))
            {
                var response = reader.Current;
                handler.Invoke("", OrderText.Parse(response.Text));
            }
        }

        public void UnsubscribeSubscriberOrderUpdate(Action<string, OrderText> handler)
        {
            using var activity = source.StartActivity("UnsubscribeSubscriberOrderUpdate");
        }
    }
}
