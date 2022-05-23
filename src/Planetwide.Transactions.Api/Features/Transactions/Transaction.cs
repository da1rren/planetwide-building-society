namespace Planetwide.Transactions.Api.Features.Transactions;

public class Transaction : INode
{
    [ID]
    public int Id { get; set; }

    public int AccountId { get; set; }

    public decimal Amount { get; set; }

    public string? Reference { get; set; }

    public string[] Tags { get; set; }
}