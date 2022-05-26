using MongoDB.Driver;

namespace Planetwide.Transactions.Api.Features.Transactions;

[ExtendObjectType(typeof(QueryRoot))]
public class TransactionQueries
{
    public IExecutable<TransactionBase> GetTransactions([Service] IMongoCollection<TransactionBase> collection,
        [ID("Account")] int accountId)
    {
        var filter = Builders<TransactionBase>.Filter
            .Eq(x => x.AccountId, accountId);

        return collection.Find(filter)
            .AsExecutable();
    }
}