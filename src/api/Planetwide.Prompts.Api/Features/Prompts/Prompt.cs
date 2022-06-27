using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Planetwide.Shared.Attributes;

namespace Planetwide.Prompts.Api.Features.Prompts;

public class Prompt : INode
{
    [PromptId]
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; init; }

    [MemberId]
    public int MemberId { get; set; }
    
    public string? Message { get; set; }

    public DateTimeOffset? DismissedOn { get; set; }
}