namespace Planetwide.Accounts.Api.Features.Accounts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Account
{
    public int Id { get; init; }

    public int MemberId { get; init; }

    public string Number { get; init; }

    public string SortCode { get; init; }

    public string Iban { get; init; }

    public decimal Balance { get; set; }

    public Account()
    {

    }
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