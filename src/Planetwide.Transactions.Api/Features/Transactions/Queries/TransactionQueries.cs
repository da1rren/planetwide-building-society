namespace Planetwide.Transactions.Api.Features.Transactions.Queries;

public class TransactionQueries
{
    public async Task<IQueryable<Transactions>> GetTransactions()
    {
        return new List<Transactions>
        {

        }
            .AsQueryable();
    }
}