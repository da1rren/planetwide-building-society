using MongoDB.Bson;
using MongoDB.Driver;

namespace Planetwide.Prompts.Api.Features.Prompts;

public class DismissedPrompt
{
    [ID("Prompt")] public ObjectId Id { get; set; }
}

[ExtendObjectType(typeof(MutationRoot))]
public class PromptMutations
{
    public async Task<DismissedPrompt> DismissPrompt([Service] IMongoCollection<Prompt> collection,
        CancellationToken cancellationToken, [ID("Prompt")] ObjectId promptId)
    {
        var filter = Builders<Prompt>.Filter
            .Eq(x => x.Id, promptId);

        var update = Builders<Prompt>.Update
            .Set(x => x.DismissedOn, DateTimeOffset.Now);

        await collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return new DismissedPrompt {Id = promptId};
    }
}