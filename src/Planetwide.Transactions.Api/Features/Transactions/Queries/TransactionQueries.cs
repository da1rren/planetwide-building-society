namespace Planetwide.Transactions.Api.Features.Transactions.Queries;

using MongoDB.Driver;

[ExtendObjectType(typeof(QueryRoot))]
public class TransactionQueries
{
    [UsePaging]
    [UseProjection]
    [UseSorting]
    [UseFiltering]
    public IExecutable<Transaction> GetTransactions([Service] IMongoCollection<Transaction> collection)
    {
        return collection.AsExecutable();
    }
}