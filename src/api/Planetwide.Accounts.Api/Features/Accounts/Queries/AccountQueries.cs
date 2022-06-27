using Planetwide.Shared.Attributes;

namespace Planetwide.Accounts.Api.Features.Accounts.Queries;

using Infrastructure.Data;

[ExtendObjectType(typeof(QueryRoot))]
public class AccountQueries
{
    [UseDbContext(typeof(AccountContext))]
    [UseSingleOrDefault]
    [UseProjection]
    public IQueryable<Account> GetAccount(AccountContext accountContext, [AccountId] int accountId)
    {
        return accountContext.Account.Where(x => x.Id == accountId);
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Account> GetAccounts(AccountContext accountContext, [MemberId] int memberId)
        => accountContext.Account.Where(x => x.MemberId == memberId);
}