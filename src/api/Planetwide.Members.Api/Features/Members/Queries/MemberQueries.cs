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
    public IQueryable<Member> GetMember(MemberContext memberContext, [ID] int memberId)
    {
        return memberContext.Members.Where(x => x.Id == memberId);
    }
}