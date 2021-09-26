namespace Trsys.CopyTrading.Abstractions
{
    public interface IEaSessionTokenProvider
    {
        string GenerateToken(string key, string keyType);
    }
}
