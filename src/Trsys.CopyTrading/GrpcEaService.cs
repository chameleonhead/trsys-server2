﻿using Google.Protobuf.WellKnownTypes;
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
        private readonly EaServicePool pool;
        private readonly IEaSessionTokenValidator tokenValidator;
        private readonly CopyTradingOrderTextCache cache;

        public GrpcEaService(EaServicePool pool, IEaSessionTokenValidator tokenValidator, CopyTradingOrderTextCache cache)
        {
            this.pool = pool;
            this.tokenValidator = tokenValidator;
            this.cache = cache;
        }

        private Task<T> ExecuteAsync<T>(Func<Ea.EaClient, Task<T>> execution)
        {
            return pool.UseClientAsync(execution);
        }

        private Task ExecuteAsync(Func<Ea.EaClient, Task> execution)
        {
            return pool.UseClientAsync(execution);
        }

        public Task AddSecretKeyAsync(string key, string keyType)
        {
            return ExecuteAsync(async service =>
            {
                using var activity = CopyTradingActivitySource.Source.StartActivity("GrpcClient.AddSecretKeyAsync");
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
            });
        }

        public async Task RemvoeSecretKeyAsync(string key, string keyType)
        {
            using var activity = CopyTradingActivitySource.Source.StartActivity("GrpcClient.RemvoeSecretKeyAsync");
            await ExecuteAsync(async service =>
            {
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
            });
        }

        public async Task<EaSession> GenerateSessionTokenAsync(string key, string keyType)
        {
            using var activity = CopyTradingActivitySource.Source.StartActivity("GrpcClient.GenerateSessionTokenAsync");
            return await ExecuteAsync(async service =>
            {
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
            });
        }

        public async Task DiscardSessionTokenAsync(string token, string key, string keyType)
        {
            using var activity = CopyTradingActivitySource.Source.StartActivity("GrpcClient.DiscardSessionTokenAsync");
            await ExecuteAsync(async service =>
            {
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
            });
        }

        public async Task ValidateSessionTokenAsync(string token, string key, string keyType)
        {
            using var activity = CopyTradingActivitySource.Source.StartActivity("GrpcClient.ValidateSessionTokenAsync");
            try
            {
                if (!tokenValidator.ValidateToken(key, keyType, token))
                {
                    throw new EaSessionTokenInvalidException();
                }
                return;
            }
            catch { }
            await ExecuteAsync(async service =>
            {
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
            });
        }

        public Task PublishOrderTextAsync(DateTimeOffset timestamp, string key, string text)
        {
            var activity = CopyTradingActivitySource.Source.StartActivity("GrpcClient.PublishOrderTextAsync");
            Task.Run(() => ExecuteAsync(async service =>
            {
                // offload to other thread
                using (activity)
                {
                    var response = await service.PublishOrderTextAsync(new PublishOrderTextRequest()
                    {
                        Timestamp = Timestamp.FromDateTimeOffset(timestamp),
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
            }));
            return Task.CompletedTask;
        }

        public async Task<OrderText> GetCurrentOrderTextAsync()
        {
            using var activity = CopyTradingActivitySource.Source.StartActivity("GrpcClient.GetCurrentOrderTextAsync");
            var orderText = cache.GetOrderTextCache();
            if (orderText != null)
            {
                activity.AddEvent(new ActivityEvent("OrderTextCacheHit"));
                return orderText;
            }
            return await ExecuteAsync(async service =>
            {
                var response = await service.GetCurrentOrderTextAsync(new GetCurrentOrderTextRequest());
                switch (response.Result)
                {
                    case GetCurrentOrderTextResponse.Types.Result.Success:
                        orderText = OrderText.Parse(response.Text);
                        cache.UpdateOrderTextCache(orderText);
                        return orderText;
                    default:
                        throw new Exception(response.Message);
                }
            });
        }

        public Task SubscribeOrderTextAsync(DateTimeOffset timestamp, string key, string text)
        {
            var activity = CopyTradingActivitySource.Source.StartActivity("GrpcClient.SubscribeOrderTextAsync");
            Task.Run(() => ExecuteAsync(async service =>
            {
                // offload to other thread
                using (activity)
                {
                    var response = await service.SubscribeOrderTextAsync(new SubscribeOrderTextRequest()
                    {
                        Timestamp = Timestamp.FromDateTimeOffset(timestamp),
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
            }));
            return Task.CompletedTask;
        }

        public async Task ReceiveLogAsync(DateTimeOffset serverTimestamp, string key, string keyType, string version, string token, string text)
        {
            using var activity = CopyTradingActivitySource.Source.StartActivity("GrpcClient.ReceiveLogAsync");
            await ExecuteAsync(async service =>
            {
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
            });
        }

        public async Task ReceiveLogAsync(DateTimeOffset serverTimestamp, long eaTimestamp, string key, string keyType, string version, string token, string text)
        {
            using var activity = CopyTradingActivitySource.Source.StartActivity("GrpcClient.ReceiveLogAsync");
            await ExecuteAsync(async service =>
            {
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
            });
        }

        public async void SubscribeOrderTextUpdated(Action<OrderText> handler)
        {
            using var activity = CopyTradingActivitySource.Source.StartActivity("GrpcClient.SubscribeSubscriberOrderUpdate");
            await ExecuteAsync(async service =>
            {
                using var stream = service.GetCurrentOrderTextStream(new GetCurrentOrderTextRequest());
                var reader = stream.ResponseStream;
                while (await reader.MoveNext(CancellationToken.None))
                {
                    var response = reader.Current;
                    handler.Invoke(OrderText.Parse(response.Text));
                }
            });
        }

        public void UnsubscribeOrderTextUpdated(Action<OrderText> handler)
        {
            using var activity = CopyTradingActivitySource.Source.StartActivity("GrpcClient.UnsubscribeSubscriberOrderUpdate");
        }
    }
}
