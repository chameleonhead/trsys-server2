namespace Trsys.CopyTrading.Abstractions
{
    public class EaSession
    {
        public EaSession(string key, string keyType, string token)
        {
            Key = key;
            KeyType = keyType;
            Token = token;
        }

        public string Key { get; }
        public string KeyType { get; }
        public string Token { get; }
    }
}