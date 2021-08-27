using System.Collections.Generic;

namespace Trsys.CopyTrading.Abstractions
{
    public class SecretKey
    {
        public string Key { get; set; }
        public string KeyType { get; set; }
        public HashSet<string> Followers { get; } = new();
    }
}
