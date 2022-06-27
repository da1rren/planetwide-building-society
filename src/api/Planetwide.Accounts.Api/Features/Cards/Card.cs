using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planetwide.Accounts.Api.Features.Accounts;
using Planetwide.Shared.Attributes;

namespace Planetwide.Accounts.Api.Features.Cards;

public class Card
{
    [CardId]
    public int Id { get; set; }
    
    [AccountId]
    public int AccountId { get; set; }

    public Account Account { get; set; } = null!;

    public string Pan { get; set; } = null!;

    public DateOnly Expiry { get; set; }
}

public class CardEntityConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Pan)
            .IsRequired()
            .HasMaxLength(32);

        builder.HasOne(x => x.Account)
            .WithMany(x => x.Cards);
    }
}