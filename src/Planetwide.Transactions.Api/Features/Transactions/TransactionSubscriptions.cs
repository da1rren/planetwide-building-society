namespace Planetwide.Transactions.Api.Features.Transactions;

public record TransactionSummary(DateTimeOffset MadeOn, decimal Amount, string Reference, string[]? Tags);

[ExtendObjectType(typeof(SubscriptionRoot))]
public class TransactionSubscriptions
{
    [Subscribe]
    public TransactionSummary TransactionAdded([EventMessage] TransactionSummary transactionSummary) => 
        transactionSummary;
}