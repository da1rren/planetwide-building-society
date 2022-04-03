namespace Planetwide.Accounts.Api.Features.Accounts.Queries;

using Infrastructure.Data;

[ExtendObjectType(typeof(QueryRoot))]
public class AccountQueries
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Account> GetAccounts(AccountContext accountContext)
        => accountContext.Account;

    [UseSingleOrDefault]
    public IQueryable<Account> GetAccount(AccountContext accountContext, int accountId)
    {
        return accountContext.Account.Where(x => x.Id == accountId);
    }

    [UseProjection]
    public IQueryable<Account> GetMemberAccounts(AccountContext accountContext, int memberId)
    {
        return accountContext.Account.Where(x => x.MemberId == memberId);
    }
}