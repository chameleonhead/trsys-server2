namespace Trsys.CopyTrading.Infrastructure
{
    public static class InMemoryKeys
    {
        public record SecretKey(string Key, string KeyType);
        public record PublisherOrderKey(string PublisherKey, int TicketNo);
    }
}
