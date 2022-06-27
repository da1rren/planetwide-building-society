using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planetwide.Shared.Attributes;

namespace Planetwide.Members.Api.Features.Members;

public class MemberMarketingPreferences
{
    [MemberMarketingId]
    public int Id { get; set; }

    public DateTimeOffset? ByPost { get; set; }
    
    public DateTimeOffset? ByTelephone { get; set; }
    
    public DateTimeOffset? ByEmail { get; set; }
    
    public DateTimeOffset? ByOnline { get; set; }
    
    public DateTimeOffset? FaceToFace { get; set; }
    
    public DateTimeOffset? ByPhone { get; set; }

    public DateTimeOffset? BySmsService { get; set; }
    
    public DateTimeOffset? BySmsMarketing { get; set; }

    public int MemberId { get; set; }
    
    public virtual Member? Member { get; set; }
}

public class MemberMarketingPreferencesConfiguration : IEntityTypeConfiguration<MemberMarketingPreferences>
{
    public void Configure(EntityTypeBuilder<MemberMarketingPreferences> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Member)
            .WithOne(x => x.Preferences)
            .HasForeignKey<MemberMarketingPreferences>(x => x.MemberId)
            .IsRequired();
    }
}