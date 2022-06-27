using MongoDB.Driver;
using Planetwide.Shared.Attributes;

namespace Planetwide.Transactions.Api.Features.Transactions;

[ExtendObjectType(typeof(QueryRoot))]
public class TransactionQueries
{
    public IExecutable<TransactionBase> GetTransactions([Service] IMongoCollection<TransactionBase> collection,
        [AccountId] int accountId)
    {
        var filter = Builders<TransactionBase>.Filter
            .Eq(x => x.AccountId, accountId);

        return collection.Find(filter)
            .AsExecutable();
    }
}