using System;
using System.Collections.Generic;

namespace Trsys.CopyTrading.Abstractions
{
    public class EaLogContext
    {
        public EaLogContext(
            DateTimeOffset serverTimestamp,
            string secretKeyId,
            string eaSessionId,
            string key,
            string keyType,
            string token,
            string version,
            string text)
        {
            ServerTimestamp = serverTimestamp;
            SecretKeyId = secretKeyId;
            EaSessionId = eaSessionId;
            Key = key;
            KeyType = keyType;
            Token = token;
            Version = version;
            Lines = text?.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
        }

        public DateTimeOffset ServerTimestamp { get; }
        public string SecretKeyId { get; }
        public string Key { get; }
        public string KeyType { get; }
        public string EaSessionId { get; }
        public string Token { get; }
        public string Version { get; }
        public IEnumerable<string> Lines { get; }
    }
}
