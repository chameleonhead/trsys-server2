using System;

namespace Trsys.Analytics.EaLogs
{
    public class InitLog : ILogInfo
    {
        public InitLog()
        {
        }

        public InitLog(DateTimeOffset timestamp, string key, string keyType, string version, string token)
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = timestamp;
            Key = key;
            KeyType = keyType;
            Version = version;
            Token = token;
        }

        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Type => "EaLogInit";
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Version { get; set; }
        public string Token { get; set; }
    }
}
