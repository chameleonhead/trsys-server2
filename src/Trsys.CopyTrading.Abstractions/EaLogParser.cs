using System;
using System.Collections.Generic;

namespace Trsys.CopyTrading.Abstractions
{
    public class EaLogParser
    {
        public static IEnumerable<IEvent> Parse(
            DateTimeOffset timestamp,
            string secretKeyId,
            string eaSessionId,
            string key,
            string keyType,
            string token,
            string version,
            string text)
        {
            return Array.Empty<IEvent>();
        }
    }
}
