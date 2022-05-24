using MongoDB.Bson.Serialization.Attributes;

namespace Planetwide.Transactions.Api.Features.Transactions;

[UnionType("Metadata")]
public interface IMetadata{}

[BsonDiscriminator(nameof(NetworkMetadata))]
public class NetworkMetadata : IMetadata
{
    public string IpAddress { get; set; }
}

[BsonDiscriminator(nameof(LatencyMetadata))]
public class LatencyMetadata : IMetadata
{
    public TimeSpan Latency { get; set; }
}

[BsonDiscriminator(nameof(RetentionMetadata))]
public class RetentionMetadata : IMetadata
{
    public DateTimeOffset DeleteOn { get; set; }
}
