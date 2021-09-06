namespace Trsys.Events.Kafka
{
    public class KafkaEventOptions
    {
        public string BootstrapServers { get; set; }
        public string Topic { get; set; } = "copy-trading-system";
    }
}
