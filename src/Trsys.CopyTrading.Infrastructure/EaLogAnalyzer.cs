using System;
using System.Text.Json;
using Trsys.CopyTrading.EaLogs;

namespace Trsys.CopyTrading.Infrastructure
{
    public class EaLogAnalyzer : IEaLogAnalyzer
    {
        public void AnalyzeLog(DateTimeOffset serverTimestamp, long eaTimestamp, string key, string keyType, string version, string token, string text)
        {
            var logs = EaLogParser.Parse(serverTimestamp, key, keyType, token, version, text);
            foreach (var log in logs)
            {
                Console.WriteLine(JsonSerializer.Serialize(log));
            }
        }

        public void AnalyzeLog(DateTimeOffset timestamp, string key, string keyType, string version, string token, string text)
        {
            var logs = EaLogParser.Parse(timestamp, key, keyType, token, version, text);
            foreach (var log in logs)
            {
                Console.WriteLine(JsonSerializer.Serialize(log));
            }
        }
    }
}