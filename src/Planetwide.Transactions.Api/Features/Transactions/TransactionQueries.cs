using MongoDB.Driver;

namespace Planetwide.Transactions.Api.Features.Transactions;

[ExtendObjectType(typeof(QueryRoot))]
public class TransactionQueries
{
    [UseProjection]
    [UseSorting]
    public IExecutable<Transaction> GetTransactions([Service] IMongoCollection<Transaction> collection,
        [ID] int accountId)
    {
        var filter = Builders<Transaction>.Filter
            .Eq(x => x.AccountId, accountId);

        return collection.Find(filter)
            .AsExecutable();
    }
}