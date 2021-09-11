namespace Trsys.CopyTrading.Abstractions
{
    public interface IEaStore
    {
        void Add(string key, string keyType);
        void Remove(string key, string keyType);
        EaBase Find(string key, string keyType);
    }
}
