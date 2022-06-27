using Planetwide.Accounts.Api.Features.Cards;
using Planetwide.Shared.Attributes;

namespace Planetwide.Accounts.Api.Features.Accounts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Account : INode
{
    [AccountId]
    public int Id { get; set; }

    [MemberId]
    public int MemberId { get; init; }

    public string Number { get; init; } = null!;

    public string SortCode { get; init; } = null!;

    public string Iban { get; init; } = null!;

    public decimal Balance { get; set; }

    public ICollection<Card> Cards { get; set; } = null!;
}

public class AccountEntityConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Number)
            .IsRequired();

        builder.Property(x => x.MemberId)
            .IsRequired();

        builder.Property(x => x.SortCode)
            .IsRequired();

        builder.HasIndex(x => new { x.Iban, x.Number })
            .IsUnique();

        builder.Property(x => x.Iban)
            .IsRequired();

        builder.HasIndex(x => x.Iban)
            .IsUnique();

        builder.Property(x => x.Balance)
            .HasPrecision(18, 7);
    }
}