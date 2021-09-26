using System;
using JWT.Algorithms;
using JWT.Builder;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class EaSessionTokenProvider : IEaSessionTokenProvider
    {
        public string GenerateToken(string key, string keyType)
        {
            return JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret("s3cr3t")
                .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(3).ToUnixTimeSeconds())
                .AddClaim("key", key)
                .AddClaim("keyType", keyType)
                .Encode();
        }
    }
}