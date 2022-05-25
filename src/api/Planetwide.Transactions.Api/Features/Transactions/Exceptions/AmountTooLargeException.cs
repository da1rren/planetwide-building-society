namespace Planetwide.Transactions.Api.Features.Transactions.Exceptions;

public class AmountTooLargeException : Exception
{
    public decimal AttemptedAmount { get; init; }

    public decimal Limit { get; init; }

    public AmountTooLargeException(decimal attemptedAmount, decimal limit) : base("You cannot transfer that amount of money")
    {
        AttemptedAmount = attemptedAmount;
        Limit = limit;
    }
}