using System;

namespace Trsys.Analytics.EaLogs
{
    public class DeinitLog : ILogInfo
    {
        public DeinitLog()
        {
        }

        public DeinitLog(DateTimeOffset timestamp, string key, string keyType, string version, string token, int reason)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = timestamp;
            Key = key;
            KeyType = keyType;
            Version = version;
            Token = token;
            Reason = reason;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "EaLogDeinit";
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Version { get; set; }
        public string Token { get; set; }
        public int Reason { get; set; }
    }
}
