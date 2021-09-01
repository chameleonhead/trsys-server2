using System;
using System.Collections.Generic;
using System.Linq;

namespace Trsys.CopyTrading.Abstractions
{
    public class EaLogParser
    {
        public static IEnumerable<IEvent> Parse(
            DateTimeOffset serverTimestamp,
            string key,
            string keyType,
            string token,
            string version,
            string text)
        {
            var context = new EaLogContext(serverTimestamp, key, keyType, token, version, text);
            var lines = context.Lines.ToArray();
            var events = new List<IEvent>();
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var (ts, level, message) = ParseLine(context, line);
            }
            return Array.Empty<IEvent>();
        }

        private static Tuple<DateTimeOffset, string, string> ParseLine(EaLogContext context, string line)
        {
            return Tuple.Create(DateTimeOffset.UtcNow, "INFO", "Message");
        }
    }
}
