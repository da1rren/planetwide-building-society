using MongoDB.Bson;
using MongoDB.Driver;
using Planetwide.Shared.Attributes;

namespace Planetwide.Prompts.Api.Features.Prompts;

public class DismissedPrompt
{
    [PromptId] public ObjectId Id { get; set; }
    
    public DateTimeOffset? DismissedOn { get; set; }
}

[ExtendObjectType(typeof(MutationRoot))]
public class PromptMutations
{
    public async Task<DismissedPrompt> DismissPrompt([Service] IMongoCollection<Prompt> collection,
        CancellationToken cancellationToken, [PromptId] ObjectId promptId)
    {
        var filter = Builders<Prompt>.Filter
            .Eq(x => x.Id, promptId);

        var dismissedAt = DateTimeOffset.Now;
        var update = Builders<Prompt>.Update
            .Set(x => x.DismissedOn, dismissedAt);

        await collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return new DismissedPrompt
        {
            Id = promptId,
            DismissedOn = dismissedAt
        };
    }
}