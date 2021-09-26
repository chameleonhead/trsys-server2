namespace Trsys.CopyTrading.Abstractions
{
    public interface IEaSessionTokenValidator
    {
        bool ValidateToken(string key, string keyType, string token);
    }
}