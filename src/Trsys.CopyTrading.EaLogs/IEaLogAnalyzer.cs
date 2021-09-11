using System;

namespace Trsys.CopyTrading.EaLogs
{
    public interface IEaLogAnalyzer
    {
        void AnalyzeLog(DateTimeOffset serverTimestamp, long eaTimestamp, string key, string keyType, string version, string token, string text);
        void AnalyzeLog(DateTimeOffset timestamp, string key, string keyType, string version, string token, string text);
    }
}
