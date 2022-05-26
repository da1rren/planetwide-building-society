using MongoDB.Bson;
using MongoDB.Driver;
using Planetwide.Prompts.Api.Features.Prompts;

namespace Planetwide.Prompts.Api.Daemon;

public class SeedJob : IHostedService
{
    private readonly IMongoCollection<Prompt> _mongoCollection;
    private Task _backgroundTask = null!;

    public SeedJob(IMongoCollection<Prompt> mongoCollection)
    {
        _mongoCollection = mongoCollection;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _backgroundTask = SeedDatabase(cancellationToken);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _backgroundTask;
    }

    private async Task SeedDatabase(CancellationToken cancellationToken)
    {
        var prompts = new List<Prompt>
        {
            new Prompt
            {
                Id = ObjectId.GenerateNewId(),
                MemberId = 1,
                Message = "Open a new saving account and get 1.2% APR",
                DismissedOn = null
            },
            new Prompt
            {
                Id = ObjectId.GenerateNewId(),
                MemberId = 1,
                Message = "You have a new custom service message",
                DismissedOn = null
            },
            new Prompt
            {
                Id = ObjectId.GenerateNewId(),
                MemberId = 1,
                Message = "Your Planetwide account was opened",
                DismissedOn = DateTimeOffset.Now.AddYears(-1)
            },
        };

        await _mongoCollection.InsertManyAsync(prompts, cancellationToken: cancellationToken);
    }
}