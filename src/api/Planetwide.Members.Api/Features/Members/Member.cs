using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planetwide.Shared.Attributes;

namespace Planetwide.Members.Api.Features.Members;

public class Member : INode
{
    [MemberId]
    public int Id { get; set; }

    public string? Firstname { get; set; }

    public string? Surname { get; set; }
    
    public virtual MemberMarketingPreferences? Preferences { get; set; }
}

public class MemberEntityConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Firstname)
            .IsRequired();

        builder.Property(x => x.Surname)
            .IsRequired();
    }
}