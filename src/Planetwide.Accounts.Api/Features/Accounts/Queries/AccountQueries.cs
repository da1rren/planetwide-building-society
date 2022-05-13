namespace Planetwide.Accounts.Api.Features.Accounts.Queries;

using HotChocolate.Resolvers;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
    public async Task<Account> GetAccount(IResolverContext context, AccountContext accountContext, int accountId)
    {
        return await context.BatchDataLoader<int, Account>(async (keys, ct) =>
        {
            return await accountContext.Account.Where(a => keys.Contains(a.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken: ct);
        }).LoadAsync(accountId);
    }

    public async Task<Account[]> GetMemberAccounts(IResolverContext context, AccountContext accountContext, int memberId)
    {
        return await context.GroupDataLoader<int, Account>( async (keys, ct) =>
        {
            var accounts = await accountContext.Account.Where(x => keys.Contains(x.Id))
                .ToListAsync(cancellationToken: ct);

            return accounts.ToLookup(x => x.MemberId);
        }).LoadAsync(memberId);
    }
}