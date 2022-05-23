using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Planetwide.Transactions.Api.Features.Transactions;

public class Transaction : INode
{
    [ID]
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    public int AccountId { get; set; }

    public decimal Amount { get; set; }

    public string? Reference { get; set; }

    public string[]? Tags { get; set; }

    public DateTimeOffset MadeOn { get; set; }
}