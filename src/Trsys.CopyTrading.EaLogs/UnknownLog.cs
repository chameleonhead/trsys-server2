using System;

namespace Trsys.CopyTrading.EaLogs
{
    public class UnknownLog : LogBase
    {
        public UnknownLog()
        {
        }

        public UnknownLog(DateTimeOffset timestamp, string key, string keyType, string version, string token, string logLevel, string message) : base(timestamp, key, keyType, version, token)
        {
            LogLevel = logLevel;
            Message = message;
        }

        public string LogLevel { get; set; }
        public string Message { get; set; }
    }
}
