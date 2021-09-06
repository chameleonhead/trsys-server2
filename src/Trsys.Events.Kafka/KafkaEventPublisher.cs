using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using Trsys.Events.Abstractions;

namespace Trsys.Events.Kafka
{
    public class KafkaEventPublisher : IEventPublisher, IDisposable
    {
        private readonly IOptions<KafkaEventOptions> options;
        private readonly IProducer<Null, string> producer;

        public KafkaEventPublisher(IOptions<KafkaEventOptions> options)
        {
            this.options = options;
            var config = new ProducerConfig
            {
                BootstrapServers = options.Value.BootstrapServers,
                ClientId = Dns.GetHostName(),
            };


            producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public void Publish<T>(T e) where T : IEvent
        {
            producer.ProduceAsync(options.Value.Topic, new Message<Null, string>() { });
        }

        public void Dispose()
        {
            producer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
