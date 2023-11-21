using Confluent.Kafka;
using Light.MessageBrokers.ApacheKafka.Options;
using Light.MessageBrokers.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Light.MessageBrokers.ApacheKafka;

public class KafkaMessageBroker : IMessageBroker
{
    private readonly KafkaOptions _kafkaOptions;
    private readonly string[] _topics;

    public KafkaMessageBroker(IOptions<KafkaOptions> options)
    {
        _kafkaOptions = options.Value;
        _topics = _kafkaOptions.Topics.Split(";");
    }

    public void Subscribe(Type type, Action<string?> callback, CancellationToken ct)
    {
        using var consumer = new ConsumerBuilder<string, string>(_kafkaOptions.Consumer).Build();

        if (consumer == null)
        {
            throw new ArgumentNullException(nameof(consumer));
        }

        consumer.Subscribe(_topics);
        
        while (!ct.IsCancellationRequested)
        {
            var message = consumer.Consume(ct);
            var obj = message.Message.Value;
            callback.Invoke(obj);
        }
    }

    public void Subscribe<T>(Action<string?> callback, CancellationToken ct) where T : class
    {
        Subscribe(typeof(T), callback, ct);
    }

    public async Task Publish<T>(T entity) where T : class
    {
        using var p = new ProducerBuilder<string, string>(_kafkaOptions.Producer).Build();

        await p.ProduceAsync(_kafkaOptions.Topic,
            new Message<string, string>
            {
                Key = MessageBrokersHelper.GetTypeName<T>(), Value = JsonConvert.SerializeObject(entity)
            });
    }

    public async Task Publish(string message, string type)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentNullException(nameof(message), "Event message can not be null.");
        }

        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentNullException(nameof(type), "Event type can not be null.");
        }

        using var p = new ProducerBuilder<string, string>(_kafkaOptions.Producer).Build();

        await p.ProduceAsync(_kafkaOptions.Topic, new Message<string, string> { Key = type, Value = message });
    }
}