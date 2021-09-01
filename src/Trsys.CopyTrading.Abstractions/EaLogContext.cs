using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Trsys.CopyTrading.Abstractions
{
    public class EaLogContext
    {
        private static readonly Regex logexp = new("(\\d+):(DEBUG|ERROR|INFO):(.*)");

        public EaLogContext(
            DateTimeOffset serverTimestamp,
            string key,
            string keyType,
            string token,
            string version,
            string text)
        {
            Key = key;
            KeyType = keyType;
            Token = token;
            Version = version;
            Lines = string.IsNullOrWhiteSpace(text) ? Array.Empty<EaLogLine>() : ParseLines(text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries));
            if (Lines.Any())
            {
                var lastLine = Lines.Last();
                if (lastLine.Timestamp.HasValue)
                {
                    var hourlyLastLineTimestamp = DateTimeOffset.FromUnixTimeSeconds(lastLine.Timestamp.Value.ToUnixTimeSeconds() / 3600 * 3600);
                    var hourlyServerTimestamp = DateTimeOffset.FromUnixTimeSeconds(serverTimestamp.ToUnixTimeSeconds() / 3600 * 3600);
                    var offsetTime = hourlyServerTimestamp - hourlyLastLineTimestamp;
                    foreach (var line in Lines)
                    {
                        line.Timestamp = line.Timestamp?.Add(offsetTime);
                    }
                }
            }
        }

        public string Key { get; }
        public string KeyType { get; }
        public string Token { get; }
        public string Version { get; }
        public IEnumerable<EaLogLine> Lines { get; }

        private List<EaLogLine> ParseLines(string[] lines)
        {
            var logLines = new List<EaLogLine>();
            if (lines.Length > 0)
            {
                var match = logexp.Match(lines[0]);
                var currentLine = match.Success ? new EaLogLine()
                {
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(match.Groups[1].Value)),
                    LogLevel = match.Groups[2].Value,
                    Message = match.Groups[3].Value,
                } : new EaLogLine()
                {
                    Message = lines[0],
                };
                logLines.Add(currentLine);
                for (var i = 1; i < lines.Length; i++)
                {
                    match = logexp.Match(lines[i]);
                    if (match.Success)
                    {
                        currentLine = new EaLogLine()
                        {
                            Timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(match.Groups[1].Value)),
                            LogLevel = match.Groups[2].Value,
                            Message = match.Groups[3].Value,
                        };
                        logLines.Add(currentLine);
                    }
                    else
                    {
                        currentLine.Message += Environment.NewLine + lines[i];
                    }
                }
            }
            return logLines;
        }
    }
}
