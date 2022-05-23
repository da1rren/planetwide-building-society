namespace Planetwide.Accounts.Api.Features.Accounts.Queries;

using Infrastructure.Data;

[ExtendObjectType(typeof(QueryRoot))]
public class AccountQueries
{
    [UseDbContext(typeof(AccountContext))]
    [UseSingleOrDefault]
    public IQueryable<Account> GetAccount(AccountContext accountContext, [ID] int accountId)
    {
        return accountContext.Account.Where(x => x.Id == accountId);
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Account> GetAccounts(AccountContext accountContext, [ID] int memberId)
        => accountContext.Account.Where(x => x.MemberId == memberId);
}