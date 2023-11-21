using Confluent.Kafka;

namespace Light.MessageBrokers.ApacheKafka.Options;

public class KafkaOptions
{
    public ConsumerConfig Consumer { get; set; } = null!;
    public ProducerConfig Producer { get; set; } = null!;
    public string Topic { get; set; } = null!;
    public string Topics { get; set; } = null!;
}