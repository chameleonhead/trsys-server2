using System;

namespace Trsys.Analytics.EaLogs
{
    public class InitLog : LogBase
    {
        public InitLog()
        {
        }

        public InitLog(DateTimeOffset timestamp, string key, string keyType, string version, string token) : base(timestamp, key, keyType, version, token)
        {
        }
    }
}
