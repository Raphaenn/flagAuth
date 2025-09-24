using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Infra;

public interface IKafkaProducer
{
    Task ProduceAsync(string topic, string key, string payload, Headers? headers, CancellationToken ct);
}

public sealed class KafkaProducer : IKafkaProducer, IDisposable
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducer(IOptions<KafkaOptions> opt)
    {
        var cfg = new ProducerConfig
        {
            BootstrapServers = opt.Value.BootstrapServers,
            Acks = Acks.All,
            EnableIdempotence = opt.Value.EnableIdempotence,
            MessageSendMaxRetries = 5,
            LingerMs = 5,
            CompressionType = Confluent.Kafka.CompressionType.Zstd
        };
        
        _producer = new ProducerBuilder<string, string>(cfg).Build();
    }
    
    public async Task ProduceAsync(string topic, string key, string payload, Headers? headers, CancellationToken ct)
    {
        var msg = new Message<string, string> { Key = key, Value = payload, Headers = headers ?? new Headers() };
        await _producer.ProduceAsync(topic, msg, ct);
    }

    public void Dispose() => _producer.Dispose();
}