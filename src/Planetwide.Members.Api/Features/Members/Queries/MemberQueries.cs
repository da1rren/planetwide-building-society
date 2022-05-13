namespace Planetwide.Members.Api.Features.Members.Queries;

using HotChocolate.Resolvers;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

[ExtendObjectType(typeof(QueryRoot))]
public class MemberQueries
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Member> GetMembers(MemberContext context)
        => context.Members;

    [UseSingleOrDefault]
    [UseProjection]
    public async Task<Member> GetMember(IResolverContext context, MemberContext memberContext, int memberId)
    {
        return await context.BatchDataLoader<int, Member>(async (keys, ct) =>
        {
            return await memberContext.Members.Where(a => keys.Contains(a.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken: ct);
        }).LoadAsync(memberId);
    }
}