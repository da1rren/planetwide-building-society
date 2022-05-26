using MongoDB.Bson;

namespace Planetwide.Transactions.Api.Daemons;

using Features.Transactions;
using MongoDB.Driver;
using Shared;

public class SeedJob : IHostedService
{
    private readonly IMongoCollection<TransactionBase> _mongoCollection;
    private Task _backgroundTask = null!;

    public SeedJob(IMongoCollection<TransactionBase> mongoCollection)
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
        var transactions = new List<TransactionBase>
        {
            new BasicTransaction
            {
                Id = ObjectId.GenerateNewId(),
                AccountId = 1,
                Amount = -20,
                Reference = "1234",
                MadeOn = DateTimeOffset.Now,
                City = "Dundee",
                Tags = new[] {"Entertainment", "Spending"},
                Metadata = new IMetadata[]
                {
                    new NetworkMetadata {IpAddress = "55.89.174.252"},
                    new LatencyMetadata {Latency = TimeSpan.FromSeconds(12)},
                    new RetentionMetadata {DeleteOn = DateTimeOffset.Now.AddYears(7)}
                }
            },
            new BasicTransaction
            {
                Id = ObjectId.GenerateNewId(),
                AccountId = 1,
                Amount = -12.4m,
                Reference = "1234",
                MadeOn = DateTimeOffset.Now,
                City = "Glasgow",
                Tags = new[] {"Eating out", "Spending"},
                Metadata = new IMetadata[]
                {
                    new NetworkMetadata {IpAddress = "55.89.174.252"},
                    new LatencyMetadata {Latency = TimeSpan.FromSeconds(12)},
                    new RetentionMetadata {DeleteOn = DateTimeOffset.Now.AddYears(7)}
                }
            },
            new BasicTransaction
            {
                Id = ObjectId.GenerateNewId(),
                AccountId = 2,
                Amount = -12.4m,
                Reference = "1234",
                City = "Aberdeen",
                MadeOn = DateTimeOffset.Now,
                Tags = new[] {"Online", "Spending"},
                Metadata = new IMetadata[]
                {
                    new NetworkMetadata {IpAddress = "196.17.65.186"},
                    new LatencyMetadata {Latency = TimeSpan.FromMilliseconds(1200)},
                    new RetentionMetadata {DeleteOn = DateTimeOffset.Now.AddYears(7)}
                }
            },
            new DirectDebitTransaction
            {
                Id = ObjectId.GenerateNewId(),
                AccountId = 2,
                Amount = -148,
                Reference = "1234",
                MadeOn = DateTimeOffset.Now,
                Merchant = "Edinburgh Council",
                Tags = new[] {"Household", "Spending"},
                Metadata = new IMetadata[]
                {
                    new NetworkMetadata {IpAddress = "12.45.123.182"},
                    new LatencyMetadata {Latency = TimeSpan.FromMilliseconds(200)},
                    new RetentionMetadata {DeleteOn = DateTimeOffset.Now.AddYears(7)}
                }
            }
        };

        foreach (var transaction in transactions)
        {
            await _mongoCollection.InsertOneAsync(
                transaction, cancellationToken: cancellationToken);
        }
    }
}