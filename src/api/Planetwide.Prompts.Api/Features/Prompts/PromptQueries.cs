using HotChocolate.Data;
using MongoDB.Driver;

namespace Planetwide.Prompts.Api.Features.Prompts;

[ExtendObjectType(typeof(QueryRoot))]
public class PromptQueries
{
    public IExecutable<Prompt> GetPrompts([Service] IMongoCollection<Prompt> collection,
        [ID("Member")] int memberId)
    {
        var filter = Builders<Prompt>.Filter
            .Eq(x => x.MemberId, memberId);

        return collection.Find(filter)
            .AsExecutable();
    }
}