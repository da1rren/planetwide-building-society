namespace Planetwide.Accounts.Api.Features.Accounts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Account : INode
{
    [ID]
    public int Id { get; set; }

    [ID("Member")]
    public int MemberId { get; init; }

    public string Number { get; init; }

    public string SortCode { get; init; }

    public string Iban { get; init; }

    public decimal Balance { get; set; }
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