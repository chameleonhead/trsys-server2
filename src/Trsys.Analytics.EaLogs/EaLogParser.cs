using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Trsys.Analytics.EaLogs
{
    public class EaLogParser
    {
        private readonly Regex regex;
        private readonly Func<EaLogContext, EaLogLine, Match, ILogInfo> eventFactory;

        private EaLogParser(string pattern, Func<EaLogContext, EaLogLine, Match, ILogInfo> eventFactory)
        {
            regex = new Regex(pattern);
            this.eventFactory = eventFactory;
        }

        private bool TryConvert(EaLogContext context, EaLogLine line, out ILogInfo ev)
        {
            var match = regex.Match(line.Message);
            ev = match.Success ? eventFactory(context, line, match) : null;
            return match.Success;
        }

        private static readonly EaLogParser[] Parsers = new EaLogParser[]
        {
            new("Init", (context, line, match) => new InitLog(line.Timestamp.Value, context.Key, context.KeyType, context.Version, context.Token)),
            new("Deinit. Reason = (\\d)", (context, line, match) => new DeinitLog(line.Timestamp.Value, context.Key, context.KeyType, context.Version, context.Token, int.Parse(match.Groups[1].Value))),
            new("Local order opened. LocalOrder = (\\d+)/(\\d+)/(.*)/([01])", (context, line, match) => new LocalOrderOpenedLog(line.Timestamp.Value, context.Key, context.KeyType, context.Version, context.Token, long.Parse(match.Groups[1].Value), long.Parse(match.Groups[2].Value), match.Groups[3].Value, (OrderType) int.Parse(match.Groups[4].Value))),
            new("Local order closed. LocalOrder = (\\d+)/(\\d+)/(.*)/([01])", (context, line, match) => new LocalOrderClosedLog(line.Timestamp.Value, context.Key, context.KeyType, context.Version, context.Token, long.Parse(match.Groups[1].Value), long.Parse(match.Groups[2].Value), match.Groups[3].Value, (OrderType) int.Parse(match.Groups[4].Value))),
            new("Server order opened. ServerOrder = (\\d+)/(.*)/([01])", (context, line, match) => new ServerOrderOpenedLog(line.Timestamp.Value, context.Key, context.KeyType, context.Version, context.Token, long.Parse(match.Groups[1].Value), match.Groups[2].Value, (OrderType) int.Parse(match.Groups[3].Value))),
            new("Server order closed. ServerOrder = (\\d+)/(.*)/([01])", (context, line, match) => new ServerOrderClosedLog(line.Timestamp.Value, context.Key, context.KeyType, context.Version, context.Token, long.Parse(match.Groups[1].Value), match.Groups[2].Value, (OrderType) int.Parse(match.Groups[3].Value))),
            new("CalculateVolume: Symbol = (.*), Margin for a lot = (\\d+(\\.\\d+)?), Step = (\\d+(\\.\\d+)?)", (context, line, match) => new OrderSetupCurrencyInfoFetchedLog(line.Timestamp.Value, context.Key, context.KeyType, context.Version, context.Token, match.Groups[1].Value, decimal.Parse(match.Groups[2].Value), decimal.Parse(match.Groups[4].Value))),
            new("CalculateVolume: Free margin = (\\d+(\\.\\d+)?), Leverage = (\\d+), Percentage = (\\d+(\\.\\d+)?), Calculated volume = (\\d+(\\.\\d+)?)", (context, line, match) => new OrderSetupMarginCalculatedLog(line.Timestamp.Value, context.Key, context.KeyType, context.Version, context.Token, decimal.Parse(match.Groups[1].Value), long.Parse(match.Groups[3].Value), decimal.Parse(match.Groups[4].Value), decimal.Parse(match.Groups[6].Value))),
        };

        public static IEnumerable<ILogInfo> Parse(
            DateTimeOffset serverTimestamp,
            string key,
            string keyType,
            string token,
            string version,
            string text)
        {
            var context = new EaLogContext(serverTimestamp, key, keyType, token, version, text);
            var events = new List<ILogInfo>();
            foreach (var line in context.Lines.ToArray())
            {
                foreach (var parser in Parsers)
                {
                    if (parser.TryConvert(context, line, out var ev))
                    {
                        events.Add(ev);
                        break;
                    }
                }
            }
            return events;
        }
    }
}
