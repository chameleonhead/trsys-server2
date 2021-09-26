using System;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class EaSessionTokenProvider : IEaSessionTokenProvider
    {
        private readonly JwtEncoder encoder;

        public EaSessionTokenProvider()
        {
            var algorithm = new HMACSHA256Algorithm();
            var serializer = new JsonNetSerializer();
            var urlEncoder = new JwtBase64UrlEncoder();
            encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
        }

        public string GenerateToken(string key, string keyType)
        {
            return encoder.Encode(new Dictionary<string, object>() {
                {"id", Guid.NewGuid().ToString()},
                {"key", key},
                {"keyType", keyType},
            }, "s3cr3t");
        }
    }
}