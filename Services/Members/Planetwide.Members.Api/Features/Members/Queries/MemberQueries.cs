namespace Planetwide.Members.Api.Features.Members.Queries;

using Infrastructure.Data;

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
    public IQueryable<Member> GetMember(MemberContext context, int memberId)
    {
        return context.Members.Where(x => x.Id == memberId);
    }
}