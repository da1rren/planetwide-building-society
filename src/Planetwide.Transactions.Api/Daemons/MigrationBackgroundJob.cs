using MongoDB.Bson;

namespace Planetwide.Transactions.Api.Daemons;

using Features.Transactions;
using MongoDB.Driver;
using Shared;

public class MigrationBackgroundJob : IHostedService
{
    private readonly IMongoCollection<TransactionBase> _mongoCollection;
    private Task _backgroundTask = null!;

    public MigrationBackgroundJob(IMongoCollection<TransactionBase> mongoCollection)
    {
        _mongoCollection = mongoCollection;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _backgroundTask = MigrateDatabase(cancellationToken);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _backgroundTask;
    }

    private async Task MigrateDatabase(CancellationToken cancellationToken)
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
                City = "Dundee"
            },
            new BasicTransaction
            {
                Id = ObjectId.GenerateNewId(),
                AccountId = 1,
                Amount = -12.4m,
                Reference = "1234",
                MadeOn = DateTimeOffset.Now,
                City = "Glasgow"
            },
            new BasicTransaction
            {
                Id = ObjectId.GenerateNewId(),
                AccountId = 2,
                Amount = -12.4m,
                Reference = "1234",
                City = "Aberdeen",
                MadeOn = DateTimeOffset.Now
            },
            new DirectDebitTransaction
            {
                Id = ObjectId.GenerateNewId(),
                AccountId = 2,
                Amount = 148,
                Reference = "1234",
                MadeOn = DateTimeOffset.Now,
                Merchant = "Edinburgh Council"
            }
        };

        foreach (var transaction in transactions)
        {
            await _mongoCollection.InsertOneAsync(
                transaction, cancellationToken: cancellationToken);
        }
    }
}