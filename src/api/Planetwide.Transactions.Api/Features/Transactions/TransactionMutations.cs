using HotChocolate.Subscriptions;
using MongoDB.Driver;
using StackExchange.Redis;

namespace Planetwide.Transactions.Api.Features.Transactions;

using Exceptions;

[ExtendObjectType(typeof(MutationRoot))]
public class TransactionMutations
{
    private const decimal Limit = 10_000;
    
    [Error(typeof(AmountTooLargeException))]
    public async Task<TransactionBase> AddTransaction(
        [Service] IMongoCollection<TransactionBase> collection, 
        [Service] ITopicEventSender sender, 
        [Service] ConnectionMultiplexer multiplexer,
        CancellationToken cancellationToken,
        [ID]int accountId, 
        decimal amount, 
        string reference, 
        string[]? tags)
    {
        if (Math.Abs(amount) > Limit)
        {
            throw new AmountTooLargeException(amount, Limit);
        }

        var transaction = new BasicTransaction
        {
            AccountId = accountId,
            Amount = amount,
            Reference = reference,
            Tags = tags,
            MadeOn = DateTimeOffset.Now
        };
        
        var summary = new TransactionSummary(transaction.MadeOn, transaction.Amount,
            transaction.Reference, transaction.Tags);
        
        await collection.InsertOneAsync(transaction, cancellationToken: cancellationToken);
        await sender.SendAsync(nameof(TransactionSubscriptions.TransactionAdded), summary,
            cancellationToken);
        
        return transaction;
    }
}