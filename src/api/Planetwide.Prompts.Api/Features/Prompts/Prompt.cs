using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Planetwide.Prompts.Api.Features.Prompts;

public class Prompt : INode
{
    [ID("Prompt")]
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; init; }

    [ID("Member")]
    public int MemberId { get; set; }
    
    public string? Message { get; set; }

    public DateTimeOffset? DismissedOn { get; set; }
}