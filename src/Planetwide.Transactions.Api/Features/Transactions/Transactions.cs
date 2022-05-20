namespace Planetwide.Transactions.Api.Features.Transactions;

public class Transactions
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public decimal Amount { get; set; }

    public string? Reference { get; set; }

    public string[] Tags { get; set; }

    public Transactions()
    {
    }
}