namespace Planetwide.Members.Api.Features.Members;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Member : INode
{
    [ID]
    public int Id { get; set; }

    public string? Firstname { get; set; }

    public string? Surname { get; set; }
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