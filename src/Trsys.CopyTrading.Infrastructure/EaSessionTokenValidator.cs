using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Infrastructure
{
    public class EaSessionTokenValidator : IEaSessionTokenValidator
    {
        class Session
        {
            public string Id { get; set; }
            public string Key { get; set; }
            public string KeyType { get; set; }
        }

        private readonly JwtDecoder decoder;

        public EaSessionTokenValidator()
        {
            var serializer = new JsonNetSerializer();
            var provider = new UtcDateTimeProvider();
            var validator = new JwtValidator(serializer, provider);
            var urlEncoder = new JwtBase64UrlEncoder();
            var algorithm = new HMACSHA256Algorithm();
            decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
        }

        public bool ValidateToken(string key, string keyType, string token)
        {
            var session = decoder.DecodeToObject<Session>(token, "s3cr3t", verify: true);
            return session.Key == key && session.KeyType == keyType;
        }
    }
}